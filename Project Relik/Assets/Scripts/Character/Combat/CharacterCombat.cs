using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField]
    private MoveSet moveSet = null;
    [SerializeField]
    private Collider2D attackBox = null;
    [SerializeField]
    private Transform pushCenter = null;
    [SerializeField]
    private VisualEffect impactEffect = null;
    [SerializeField]
    private bool attackReady = true;

    private Animator animator = null;
    private Collider2D[] impactTargets = null;
    private Attack currentAttack = null;


    public bool AttackReady
    { get { return attackReady; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        impactTargets = new Collider2D[3];
    }

    public void Attack(int index)
    {
        if (!attackReady)
        {
            return;
        }

        if (!moveSet && moveSet.Moves.Length <= index && moveSet.Moves[index])
        {
            Debug.LogError("Move set currupted");
        }

        currentAttack = moveSet.Moves[index];
        if (currentAttack.CheckCoolDown())
        {
            animator.SetTrigger(moveSet.Moves[index].name);
            currentAttack.ResetCooldown();
        }
    }

    public void CheckMeleeImpact()
    {
        if (!attackBox)
        {
            return;
        }

        for (int i = 0; i < impactTargets.Length; i++)
        {
            impactTargets[i] = null;
        }

        ContactFilter2D filter = new ContactFilter2D();
        attackBox.OverlapCollider(filter, impactTargets);

        foreach (Collider2D impactTarget in impactTargets)
        {
            if (!impactTarget)
            {
                break;
            }

            var target = impactTarget.attachedRigidbody?.GetComponent<Killable>();

            if (target && target.gameObject != gameObject && !target.IsImmune(gameObject) && currentAttack != null)
            {
                Vector3 pushVector = impactTarget.bounds.center - pushCenter.position;

                var pushVector2D = new Vector2(Mathf.Round(pushVector.x), Mathf.Round(pushVector.y));
                target.Push(currentAttack.PushForce, pushVector2D.normalized);

                if (currentAttack.DamageEffect && impactEffect)
                {
                    impactEffect.visualEffectAsset = currentAttack.DamageEffect;
                    impactEffect.transform.parent = null;
                    impactEffect.transform.position = impactTarget.bounds.center;
                }

                target.TakeDamage(currentAttack.Damage, impactEffect);
            }
        }
    }
}
