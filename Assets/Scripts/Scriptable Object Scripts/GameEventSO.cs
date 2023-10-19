using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Event")]
public class GameEventSO : ScriptableObject
{
    private List<GameEventListener> listenerList = new List<GameEventListener>();

    public void Raise(object sender, object data)
    {
        for (int i = listenerList.Count - 1; i >= 0; i--)
        {
            listenerList[i].OnEventRaised(sender, data);
        }
    }
    
    public void Raise()
    {
        for (int i = listenerList.Count - 1; i >= 0; i--)
        {
            listenerList[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listenerList.Contains(listener))
        {
            listenerList.Add(listener);
        }
    }
    
    public void UnregisterListener(GameEventListener listener)
    {
        if (listenerList.Contains(listener))
        {
            listenerList.Remove(listener);
        }
    }
}
