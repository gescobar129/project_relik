using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent triggerEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerEvent?.Invoke();
    }
}
