using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class CustomUnityEvent : UnityEvent<object, object> {}

public class GameEventListener : MonoBehaviour
{
    public GameEventSO GameEvent;
    public CustomUnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(object sender, object data)
    {
        Response.Invoke(sender, data);
    }
    
    public void OnEventRaised()
    {
        Response.Invoke(null, null);
    }
}
