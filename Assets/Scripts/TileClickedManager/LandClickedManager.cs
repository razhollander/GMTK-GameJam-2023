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

    public static LandClickedManager Instance;
    private Land _currentLandMouseDown;
    private Land _currentLandMouseHover;
    
    private CancellationTokenSource _landMouseDownCancellationToken;
    private Texture2D _idleMouseTexture;
    private void Awake()
    {
        Instance = this;
    }

    public void LandMouseExit(Land land)
    {
        _currentLandMouseHover = null;
        DisableForestLongClickEffect();
        _currentLandMouseDown = null;
        ChangeCursorImage(null);
        Debug.Log(nameof(LandMouseExit));
    }

    private void DisableForestLongClickEffect()
    {
        if (_currentLandMouseDown != null && _currentLandMouseDown.BuildingType == BuildingType.None)
        {
            _landMouseDownCancellationToken?.Cancel();
            Debug.Log("Cancel!");
            LongClickEffect.Instance.SetActive(false);
            ChangeCursorImage(null);
        }
    }
    
    public void LandMouseUp(Land land)
    {
        DisableForestLongClickEffect();

        Debug.Log(nameof(LandMouseUp));
    }

    public async UniTask LandMouseDown(Land land)
    {
        Debug.Log(nameof(LandMouseDown));
        _currentLandMouseDown = land;
        
        switch (land.BuildingType)
        {
            case BuildingType.None:
                _landMouseDownCancellationToken?.Cancel();
                _landMouseDownCancellationToken = new CancellationTokenSource();
                LongClickEffect.Instance.SetActive(true);
                ChangeCursorImage(_createForestCursorTexture);

                await LongClickEffect.Instance.StartFillBar(_buildForestDuration, _landMouseDownCancellationToken);
                
                if(_landMouseDownCancellationToken.IsCancellationRequested)
                    return;
                ChangeCursorImage(null);
                LongClickEffect.Instance.SetActive(false);
                CreateForestInCurrentLand();
                _currentLandMouseDown = null;
                break;
            case BuildingType.Building:
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
        ChangeCursorImage(_hitBuildingCursorTexture);

    }

    private void ChangeCursorImage(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void LandMouseEnter(Land land)
    {
        _currentLandMouseHover = land;
 
        switch (land.BuildingType)
        {
            case BuildingType.Forest:
                ChangeCursorImage(null);
                break;
            case BuildingType.Building:
                ChangeCursorImage(_hoverBuildingCursorTexture);
                break;
            case BuildingType.None: 
                ChangeCursorImage(_hoverLandCursorTexture);
                break;
        }
    }

    private void Update()
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
