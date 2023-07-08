using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Planet;
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

    public static LandClickedManager Instance;
    private Land _currentLandMouseDown;
    private Land _currentLandMouseHover;
    
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
        Debug.Log(nameof(LandMouseExit));
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
            _mouseCurrentElementEffect = ElementEffect.None;
            
        }
        
        switch (land.BuildingType)
        {
            case BuildingType.None:
                _landMouseDownCancellationToken?.Cancel();
                _landMouseDownCancellationToken = new CancellationTokenSource();
                LongClickEffect.Instance.SetActive(true);
                _isMouseCurrentlyDownCreateForest = true;
                UpdateToCorrectCursorImage();
                await LongClickEffect.Instance.StartFillBar(_buildForestDuration, _landMouseDownCancellationToken);
                _isMouseCurrentlyDownCreateForest = false;
                UpdateToCorrectCursorImage();
                if (_landMouseDownCancellationToken.IsCancellationRequested)
                {
                    return;
                }
                
                LongClickEffect.Instance.SetActive(false);
                CreateForestInCurrentLand();
                _currentLandMouseDown = null;
                break;
            case BuildingType.Building:
                _isMouseCurrentlyDownHitBuilding = true;
                UpdateToCorrectCursorImage();
                HitBuildingsInCurrentLand();
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
        _currentLandMouseDown.HitBuilding().Forget();
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
