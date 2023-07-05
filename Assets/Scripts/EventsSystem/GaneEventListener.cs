using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GaneEventListener : MonoBehaviour
{
    public void Start()
    {
        GameManager.Instance.GameEventsSystem.AddListener(this);
    }

    public abstract void OnGameEvent(GameEvent gameEvent);
    
    public void OnDisable()
    {
        GameManager.Instance.GameEventsSystem.RemoveListener(this);
    }
}

public enum GameEvent
{
    Flood = 0,
    Electricity = 1,
    Alarm = 2,
    Freeze = 3
}