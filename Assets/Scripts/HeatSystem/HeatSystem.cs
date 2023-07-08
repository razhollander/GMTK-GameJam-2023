using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HeatSystem : MonoBehaviour
{
    public static HeatSystem Instance;
    [SerializeField] private int _checkHeatIntervalInSeconds = 5;
    private int _currentHeat = 50;
    public int CurrentHeat
    {
        get
        {
            return _currentHeat;
        }
        private set
        {
            _currentHeat = Math.Clamp(value, 0, 100);
        }
        
    }
    private List<IHeatProvider> _heatProviders = new ();
    private List<IHeatIntervalObserver> _heatIntervalObservers =  new ();
        
    public void AddHeatProvider(IHeatProvider heatProvider)
    {
        _heatProviders.Add(heatProvider);
    }
    
    public void RemoveHeatProvider(IHeatProvider heatProvider)
    {
        _heatProviders.Remove(heatProvider);
    }
    
    public void AddHeatIntervalObserver(IHeatIntervalObserver heatIntervalObserver)
    {
        _heatIntervalObservers.Add(heatIntervalObserver);
    }
    
    public void RemoveHeatIntervalObserver(IHeatIntervalObserver heatIntervalObserver)
    {
        _heatIntervalObservers.Remove(heatIntervalObserver);
    }

    
    public void Awake()
    {
        Instance = this;

        try
        {
            CheckHeatInterval().Forget();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            throw;
        }
    }

    private async UniTask CheckHeatInterval()
    {
        while (true)
        {
            await UniTask.Delay(_checkHeatIntervalInSeconds * 1000);
            
            var heatTotalDelta = 0;
            _heatProviders.ForEach(x =>
            {
                heatTotalDelta += x.HeatProvided;
                x.OnHeatInterval();
            });
            CurrentHeat += heatTotalDelta;
            _heatIntervalObservers.ForEach(x => x.OnHeatInterval(CurrentHeat, heatTotalDelta));
        }
    }
}
