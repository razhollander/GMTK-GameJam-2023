using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Outlines.Scripts.RendererFeatures
{
    public class ScreenSpaceOutlines : ScriptableRendererFeature
    {
        private static readonly int Color = Shader.PropertyToID("_Color");
        private static readonly int DepthThreshold = Shader.PropertyToID("_DepthThreshold");
        private static readonly int NormalThreshold = Shader.PropertyToID("_NormalThreshold");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int OutlineScale = Shader.PropertyToID("_OutlineScale");
        private static readonly int RobertsCrossMultiplier = Shader.PropertyToID("_RobertsCrossMultiplier");
        private static readonly int SteepAngleMultiplier = Shader.PropertyToID("_SteepAngleMultiplier");
        private static readonly int SteepAngleThreshold = Shader.PropertyToID("_SteepAngleThreshold");
    
        [SerializeField] private LayerMask outlinesLayerMask;
        [SerializeField] private LayerMask outlinesOccluderLayerMask;
        [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        [SerializeField] private ScreenSpaceOutlineSettings outlineSettings = new();
        [SerializeField] private ViewSpaceNormalsTextureSettings viewSpaceNormalsTextureSettings = new();
    
        private ScreenSpaceOutlinePass _screenSpaceOutlinePass;
        private ViewSpaceNormalsTexturePass _viewSpaceNormalsTexturePass;

        public override void Create()
        {
            if (renderPassEvent < RenderPassEvent.BeforeRenderingPrePasses)
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPrePasses;
            }

            _viewSpaceNormalsTexturePass = new ViewSpaceNormalsTexturePass(renderPassEvent, outlinesLayerMask, outlinesOccluderLayerMask, viewSpaceNormalsTextureSettings);
            _screenSpaceOutlinePass = new ScreenSpaceOutlinePass(renderPassEvent, outlineSettings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_viewSpaceNormalsTexturePass);
            renderer.EnqueuePass(_screenSpaceOutlinePass);
        }

        private class ScreenSpaceOutlinePass : ScriptableRenderPass
        {
            private const string ProfilingSamplerName = "ScreenSpaceOutlines"; 
            private readonly int TemporaryBufferID = Shader.PropertyToID("_TemporaryBuffer");
            private readonly Shader OutlineShader = Shader.Find("Hidden/Outlines");
        
            private readonly Material _screenSpaceOutlineMaterial;

            private RenderTargetIdentifier _cameraColorTarget;

            private RenderTargetIdentifier _temporaryBuffer;

            public ScreenSpaceOutlinePass(RenderPassEvent renderPassEvent, ScreenSpaceOutlineSettings settings)
            {
                this.renderPassEvent = renderPassEvent;

                _screenSpaceOutlineMaterial = new Material(OutlineShader);
                _screenSpaceOutlineMaterial.SetColor(OutlineColor, settings.outlineColor);
                _screenSpaceOutlineMaterial.SetFloat(OutlineScale, settings.outlineScale);

                _screenSpaceOutlineMaterial.SetFloat(DepthThreshold, settings.depthThreshold);
                _screenSpaceOutlineMaterial.SetFloat(RobertsCrossMultiplier, settings.robertsCrossMultiplier);

                _screenSpaceOutlineMaterial.SetFloat(NormalThreshold, settings.normalThreshold);

                _screenSpaceOutlineMaterial.SetFloat(SteepAngleThreshold, settings.steepAngleThreshold);
                _screenSpaceOutlineMaterial.SetFloat(SteepAngleMultiplier, settings.steepAngleMultiplier);
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                var temporaryTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                temporaryTargetDescriptor.depthBufferBits = 0;
                cmd.GetTemporaryRT(TemporaryBufferID, temporaryTargetDescriptor, FilterMode.Bilinear);
                _temporaryBuffer = new RenderTargetIdentifier(TemporaryBufferID);

                _cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (!_screenSpaceOutlineMaterial)
                {
                    return;
                }

                var cmd = CommandBufferPool.Get();

                using (new ProfilingScope(cmd, new ProfilingSampler(ProfilingSamplerName)))
                {
                    Blit(cmd, _cameraColorTarget, _temporaryBuffer);
                    Blit(cmd, _temporaryBuffer, _cameraColorTarget, _screenSpaceOutlineMaterial);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(TemporaryBufferID);
            }
        }

        [System.Serializable]
        private class ScreenSpaceOutlineSettings
        {
            [Header("General Outline Settings")]
            public Color outlineColor = UnityEngine.Color.black;

            [Header("Depth Settings")]
            [Range(0.0f, 100.0f)]
            public float depthThreshold = 1.5f;

            [Header("Normal Settings")]
            [Range(0.0f, 1.0f)]
            public float normalThreshold = 0.4f;

            [Range(0.0f, 20.0f)]
            public float outlineScale = 1.0f;

            [Range(0.0f, 500.0f)]
            public float robertsCrossMultiplier = 100.0f;

            [Range(0.0f, 500.0f)]
            public float steepAngleMultiplier = 25.0f;

            [Header("Depth Normal Relation Settings")]
            [Range(0.0f, 2.0f)]
            public float steepAngleThreshold = 0.2f;
        }

        private class ViewSpaceNormalsTexturePass : ScriptableRenderPass
        {
            private const string NormalsShaderProperty = "_SceneViewSpaceNormals";
            private readonly Shader ViewSpaceNormalsShader = Shader.Find("Hidden/ViewSpaceNormals");
            private readonly Shader UnlitColorShader = Shader.Find("Hidden/UnlitColor");
            private readonly ProfilingSampler ProfilingSampler = new ("SceneViewSpaceNormalsTextureCreation");
            private readonly List<ShaderTagId> _shaderTagIdList = new()
            {
                new("UniversalForward"),
                new("UniversalForwardOnly"),
                new("LightweightForward"),
                new("SRPDefaultUnlit")
            };
        
            private readonly Material _normalsMaterial;
            private readonly Material _occludersMaterial;
            private readonly RenderTargetHandle _normals;
            private readonly ViewSpaceNormalsTextureSettings _normalsTextureSettings;
            private FilteringSettings _filteringSettings;
            private FilteringSettings _occluderFilteringSettings;

            public ViewSpaceNormalsTexturePass(RenderPassEvent renderPassEvent, LayerMask layerMask, LayerMask occluderLayerMask, ViewSpaceNormalsTextureSettings settings)
            {
                this.renderPassEvent = renderPassEvent;
                _normalsTextureSettings = settings;
                _filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
                _occluderFilteringSettings = new FilteringSettings(RenderQueueRange.opaque, occluderLayerMask);

                _normals.Init(NormalsShaderProperty);
                _normalsMaterial = new Material(ViewSpaceNormalsShader);

                _occludersMaterial = new Material(UnlitColorShader);
                _occludersMaterial.SetColor(Color, _normalsTextureSettings.backgroundColor);
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                var normalsTextureDescriptor = cameraTextureDescriptor;
                normalsTextureDescriptor.colorFormat = _normalsTextureSettings.colorFormat;
                normalsTextureDescriptor.depthBufferBits = _normalsTextureSettings.depthBufferBits;
                cmd.GetTemporaryRT(_normals.id, normalsTextureDescriptor, _normalsTextureSettings.filterMode);

                ConfigureTarget(_normals.Identifier());
                ConfigureClear(ClearFlag.All, _normalsTextureSettings.backgroundColor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (!_normalsMaterial || !_occludersMaterial)
                {
                    return;
                }

                var cmd = CommandBufferPool.Get();

                using (new ProfilingScope(cmd, ProfilingSampler))
                {
                    context.ExecuteCommandBuffer(cmd);
                    cmd.Clear();

                    var drawSettings = CreateDrawingSettings(_shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                    drawSettings.perObjectData = _normalsTextureSettings.perObjectData;
                    drawSettings.enableDynamicBatching = _normalsTextureSettings.enableDynamicBatching;
                    drawSettings.enableInstancing = _normalsTextureSettings.enableInstancing;
                    drawSettings.overrideMaterial = _normalsMaterial;

                    var occluderSettings = drawSettings;
                    occluderSettings.overrideMaterial = _occludersMaterial;

                    context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref _filteringSettings);
                    context.DrawRenderers(renderingData.cullResults, ref occluderSettings, ref _occluderFilteringSettings);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(_normals.id);
            }
        }

        [System.Serializable]
        private class ViewSpaceNormalsTextureSettings
        {
            public bool enableDynamicBatching;
            public bool enableInstancing;
            public Color backgroundColor = UnityEngine.Color.black;
            public FilterMode filterMode;
            public int depthBufferBits = 16;

            [Header("View Space Normal Texture Object Draw Settings")]
            public PerObjectData perObjectData;

            [Header("General Scene View Space Normal Texture Settings")]
            public RenderTextureFormat colorFormat;
        }
    }
}