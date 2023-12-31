using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Audio;
using Cysharp.Threading.Tasks;
using Planet;
using Unity.Mathematics;
using UnityEngine;

public class LandClickedManager : MonoBehaviour
{
    [SerializeField] private float _buildForestDuration;
    [SerializeField] private Texture2D _createForestCursorTexture;
    [SerializeField] private Texture2D _hoverLandCursorTexture;
    [SerializeField] private Texture2D _hoverBuildingCursorTexture;
    [SerializeField] private Texture2D _hitBuildingCursorTexture;
    [SerializeField] private Texture2D _meteorBuildingCursorTexture;
    [SerializeField] private Texture2D _tornadoBuildingCursorTexture;

    [SerializeField] private ParticleSystem mousePoof;
    public static LandClickedManager Instance;
    private Land _currentLandMouseDown;
    private Land _currentLandMouseHover;

    private bool _isCurrentlyDoingLongClickEffectFlag = false;
    private CancellationTokenSource _landMouseDownCancellationToken;
    private Texture2D _idleMouseTexture;
    private bool _isMouseCurrentlyDownHitBuilding;
    private bool _isMouseCurrentlyDownCreateForest;
    private ElementEffect _mouseCurrentElementEffect = ElementEffect.None;

    private void Awake()
    {
        Instance = this;
    }
    
    public void LandMouseExit(Land land)
    {
        _currentLandMouseHover.BuildingTypeChangedEvent -= UpdateIdleCursorTexture;
        _currentLandMouseHover = null;
        UpdateIdleCursorTexture();
        UpdateToCorrectCursorImage();
        DisableForestLongClickEffect();
        _currentLandMouseDown = null;
    }

    private void DisableForestLongClickEffect()
    {
        if (_currentLandMouseDown != null && _currentLandMouseDown.BuildingType == BuildingType.None)
        {
            _landMouseDownCancellationToken?.Cancel();
            LongClickEffect.Instance.SetActive(false);
        }
    }
    
    public void LandMouseUp(Land land)
    {
        DisableForestLongClickEffect();
        _isMouseCurrentlyDownHitBuilding = false;
        _isMouseCurrentlyDownCreateForest = false;
        UpdateToCorrectCursorImage();
    }

    public async UniTask LandMouseDown(Land land)
    {
        _currentLandMouseDown = land;

        if (_mouseCurrentElementEffect != ElementEffect.None)
        {
            if (_mouseCurrentElementEffect == ElementEffect.Tornado)
            {
                ElementsManager.Instance.CreateTornado(land);
            }

            if (_mouseCurrentElementEffect == ElementEffect.Meteor)
            {
                ElementsManager.Instance.CreateMeteors(land);
            }
            _mouseCurrentElementEffect = ElementEffect.None;
            
        }
        
        switch (land.BuildingType)
        {
            case BuildingType.None:
                LongClickEffect.Instance.SetActive(true);
                _landMouseDownCancellationToken?.Cancel();
                _landMouseDownCancellationToken = new CancellationTokenSource();
                var thisToken = _landMouseDownCancellationToken;
                _isMouseCurrentlyDownCreateForest = true;
                UpdateToCorrectCursorImage();

                try
                {
                    var task = LongClickEffect.Instance.StartFillBar(_buildForestDuration, thisToken);
                    await task;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);

                    throw;
                }
                _isMouseCurrentlyDownCreateForest = false;
                UpdateToCorrectCursorImage();
                if (thisToken.IsCancellationRequested)
                {
                    // Debug.Log("IsCancellationRequested!");
                    return;
                }
                // Debug.Log("CreateForestInCurrentLand!");

                LongClickEffect.Instance.SetActive(false);
                CreateForestInCurrentLand();
                _currentLandMouseDown = null;
                break;
            case BuildingType.Building:
                _isMouseCurrentlyDownHitBuilding = true;
                UpdateToCorrectCursorImage();
                HitBuildingsInCurrentLand();
                
                
                RaycastHit hitInfo;

                Vector2 mousePosition = Input.mousePosition;

                Ray rayOrigin = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(rayOrigin, out hitInfo))
                {
                    if (hitInfo.transform.GetComponent<MeshRenderer>() != null)
                    {
                        Instantiate(mousePoof, hitInfo.point, quaternion.identity);
                    }
                   
                }
                break;
            
            case BuildingType.Forest: 
                break;
        }
    }

    private void CreateForestInCurrentLand()
    {
        _currentLandMouseDown.BuildForest();
    }

    private void HitBuildingsInCurrentLand()
    {
        //ShakeCamera.Instance.Shake(true);
        _currentLandMouseDown.HitBuilding().Forget();
        AudioManager.Instance.Play(AudioManager.SoundsType.HitBuilding);
    }

    private void ChangeToCursorImage(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void LandMouseEnter(Land land)
    {
        _currentLandMouseHover = land;
        _currentLandMouseHover.BuildingTypeChangedEvent += UpdateIdleCursorTexture;

        UpdateIdleCursorTexture();
        UpdateToCorrectCursorImage();
    }

    public void ReceiveElement(ElementEffect elementEffect)
    {
        _mouseCurrentElementEffect = elementEffect;
        UpdateToCorrectCursorImage();
    }
    
    private void UpdateToCorrectCursorImage()
    {
        if (_mouseCurrentElementEffect != ElementEffect.None)
        {
            switch (_mouseCurrentElementEffect)
            {
                case ElementEffect.Meteor: ChangeToCursorImage(_meteorBuildingCursorTexture); break;
                case ElementEffect.Tornado: ChangeToCursorImage(_tornadoBuildingCursorTexture); break;
            }
            
            return;
        }
        
        if (_isMouseCurrentlyDownCreateForest)
        {
            ChangeToCursorImage(_createForestCursorTexture);
            return;
        }
        
        if (_isMouseCurrentlyDownHitBuilding)
        {
            ChangeToCursorImage(_hitBuildingCursorTexture);
            return;
        }
        
        ChangeToCursorImage(_idleMouseTexture);
    }
    
    private void UpdateIdleCursorTexture()
    {
        if (_currentLandMouseHover == null)
        {
            _idleMouseTexture = null;
            return;
        }
        
        switch (_currentLandMouseHover.BuildingType)
        {
            case  BuildingType.Building:
                _idleMouseTexture = _hoverBuildingCursorTexture;
                break;
            case  BuildingType.Forest: 
                _idleMouseTexture = null;
                break;
            case  BuildingType.None:
                _idleMouseTexture = _hoverLandCursorTexture;
                break;
        }
    }
}
