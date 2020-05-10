using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "New Event", order = 0)]
public class Event : ScriptableObject
{
    [SerializeField] List<EventListener> eListeners;
    public void Register(EventListener listener)
    {
        eListeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        eListeners.Remove(listener);
    }

    public void TriggerEvent()
    {
        foreach(EventListener el in eListeners)
        {
            el.OnEventTrigger();
        }
    }
}
