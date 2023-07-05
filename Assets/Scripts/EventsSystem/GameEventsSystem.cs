using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsSystem
{
    private List<GaneEventListener> _listeners = new List<GaneEventListener>();

    public void FireGameEvent(GameEvent gameEvent)
    {
        foreach (var listener in _listeners)
        {
            listener.OnGameEvent(gameEvent);
        }
    }
    
    public void AddListener(GaneEventListener listener)
    {
        _listeners.Add(listener);
    }
    
    public void RemoveListener(GaneEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
