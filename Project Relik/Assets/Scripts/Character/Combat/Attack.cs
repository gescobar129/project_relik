using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Attack", menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    [SerializeField]
    private int damage = 0;
    [SerializeField]
    private float pushForce = 0;
    [SerializeField]
    private float coolDown = 0;
    [SerializeField]
    private VisualEffectAsset damageEffectAsset = null;
    [SerializeField]
    private VisualEffectAsset contactEffectAsset = null;

    private float timeUsed = -1;

    public int Damage
    {
        get { return damage; }
    }

    public float PushForce
    {
        get { return pushForce; }
    }

    public VisualEffectAsset DamageEffect
    {
        get { return damageEffectAsset; }
    }

    public VisualEffectAsset ContactEffect
    {
        get { return contactEffectAsset; }
    }

    public void OnEnable()
    {
        timeUsed = -1;
    }

    public bool CheckCoolDown()
    {
        if (timeUsed < 0)
        {
            return true;
        }

        return Time.realtimeSinceStartup - timeUsed >= coolDown;
    }

    public void ResetCooldown()
    {
        timeUsed = Time.realtimeSinceStartup;
    }
}
