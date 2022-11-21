using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICollisionEventTarget : IEventSystemHandler
{
    // functions that can be called via the messaging system
    void TriggerEnter2D(Collider2D childCollider, Collider2D collider);
    void TriggerExit2D(Collider2D childCollider, Collider2D collider);
}

public class CollisionChild : MonoBehaviour
{
    private Collider2D boundingCollider = null;

    public void Awake()
    {
        boundingCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ExecuteEvents.Execute<ICollisionEventTarget>(transform.parent.gameObject, null, (x, y) => x.TriggerEnter2D(boundingCollider, collision));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExecuteEvents.Execute<ICollisionEventTarget>(transform.parent.gameObject, null, (x, y) => x.TriggerExit2D(boundingCollider, collision));
    }
}
