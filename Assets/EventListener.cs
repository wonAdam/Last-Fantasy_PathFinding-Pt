using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] Event listeningEvent;
    [SerializeField] UnityEvent events;

    // Start is called before the first frame update
    private void OnEnable() {
        listeningEvent.Register(this);
    }
    private void OnDisable() {
        listeningEvent.Unregister(this);
    }

    public void OnEventTrigger()
    {
        events.Invoke();
    }
}
