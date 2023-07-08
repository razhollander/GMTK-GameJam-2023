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
    
    public static LandClickedManager Instance;
    private Land _currentLandMouseDown;
    private CancellationTokenSource _landMouseDownCancellationToken;

    private void Awake()
    {
        Instance = this;
    }

    public void LandMouseExit(Land land)
    {
        DisableForestLongClickEffect();
        _currentLandMouseDown = null;

        Debug.Log(nameof(LandMouseExit));
    }

    private void DisableForestLongClickEffect()
    {
        if (_currentLandMouseDown != null && _currentLandMouseDown.BuildingType == BuildingType.None)
        {
            _landMouseDownCancellationToken?.Cancel();
            Debug.Log("Cancel!");
            LongClickEffect.Instance.SetActive(false);
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
                await LongClickEffect.Instance.StartFillBar(_buildForestDuration, _landMouseDownCancellationToken);
                
                if(_landMouseDownCancellationToken.IsCancellationRequested)
                    return;
                LongClickEffect.Instance.SetActive(false);
                CreateForestInCurrentLand();
                _currentLandMouseDown = null;
                break;
            case BuildingType.Building: 
                break;
            case BuildingType.Forest: 
                break;
        }
    }

    private void CreateForestInCurrentLand()
    {
        _currentLandMouseDown.BuildForest(1);
    }
}
