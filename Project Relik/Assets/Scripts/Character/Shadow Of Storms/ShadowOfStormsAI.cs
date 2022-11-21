using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowOfStormsAI : MonoBehaviour, ICollisionEventTarget
{
    [SerializeField]
    private Collider2D frontAttackBox = null;
    [SerializeField]
    private Collider2D backAttackBox = null;
    [SerializeField]
    private Collider2D groundPoundBox = null;
    [SerializeField]
    private Collider2D anderBox = null;
    [SerializeField]
    private float angeredDiration = 1f;

    private CharacterCombat characterCombat = null;
    private CharacterMovement characterMovement = null;
    private bool enemyInRange = false;
    private bool enemyInGoundPoundRange = false;
    private bool angered = false;
    private int magicBlastCount = 0;
    private float angeredTime = 0f;
    private bool targetInAngerRange = false;

    void Awake()
    {
        characterCombat = GetComponent<CharacterCombat>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (angered)
        {
            characterMovement.Run(characterMovement.CurrentDirection);
            if (targetInAngerRange)
            {
                characterCombat.Attack(1);
                Calm();
            }

            angeredTime += Time.deltaTime;
            if (angeredTime >= angeredDiration)
            {
                Calm();
            }
        }
        else
        {
            if (enemyInGoundPoundRange)
            {
                characterCombat.Attack(1);
            }

            else if (enemyInRange && characterCombat.AttackReady)
            {
                characterCombat.Attack(0);
            }

            if (magicBlastCount >= 2)
            {
                angered = true;
            }
        }
    }

    public void CountMagicBlast()
    {
        magicBlastCount += 1;
    }

    public void TriggerEnter2D(Collider2D childCollider, Collider2D collider)
    {
        var target = collider.GetComponent<Killable>();
        if (!target || target.CombatTeam != Killable.Team.Heros)
        {
            return;
        }

        if (childCollider == groundPoundBox)
        {
            enemyInGoundPoundRange = true;
        }
        if (childCollider == frontAttackBox || childCollider == backAttackBox)
        {
            if (childCollider == backAttackBox)
            {
                characterMovement.ToggleDirection();
            }
            enemyInRange = true;
        }

        if (childCollider == anderBox)
        {
            targetInAngerRange = true;
        }
    }

    public void Calm()
    {
        angeredTime = 0;
        angered = false;
        magicBlastCount = 0;
        characterMovement.Idle();
    }

    public void TriggerExit2D(Collider2D childCollider, Collider2D collider)
    {
        var target = collider.GetComponent<Killable>();
        if (!target || target.CombatTeam != Killable.Team.Heros)
        {
            return;
        }

        if (childCollider == groundPoundBox)
        {
            enemyInGoundPoundRange = false;
        }

        if (childCollider == frontAttackBox || childCollider == backAttackBox)
        {
            enemyInRange = false;
        }

        if (childCollider == anderBox)
        {
            targetInAngerRange = false;
        }
    }
}
