using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Hero")
        {
            return;
        }

        var itemEvent = new ItemCollectedGlobalEvent.EventArgs();
        itemEvent.collected = gameObject;
        itemEvent.collector = collision.gameObject;
        GlobalEventSystem.GetInstance().TriggerEvent(ItemCollectedGlobalEvent.GetInstance(), itemEvent);

        Destroy(gameObject);
    }
}
