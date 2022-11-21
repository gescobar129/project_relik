using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMount : MonoBehaviour
{
    [SerializeField]
    private Weapon sword = null;
    [SerializeField]
    private SwordWielder hero = null;

    private void Start()
    {
        sword.Float = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var wielder = collision.GetComponent<SwordWielder>();

        if (wielder == hero)
        {
            wielder.CurrentWeapon = sword;
            sword.Float = false;
        }
    }
}
