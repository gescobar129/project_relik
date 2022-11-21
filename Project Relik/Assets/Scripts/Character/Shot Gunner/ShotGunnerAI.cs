using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShotGunnerAI : CharacterPatrolAI, ICollisionEventTarget
{
    [SerializeField]
    private Collider2D dangerBox = null;
    [SerializeField]
    private float shotDelay = 0;

    private bool enemyInRange = false;
    private float shotTime = 0;
    private CharacterCombat characterCombat = null;

    private void Awake()
    {
        AIUnityAwake();
        characterCombat = GetComponent<CharacterCombat>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AIUnityStart();
    }

    void Update()
    {
        if (enemyInRange)
        {
            shotTime += Time.deltaTime;

            if (shotTime >= shotDelay)
            {
                characterCombat.Attack(0);
            }
        }

        AIUnityUpdate();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        AIUnityOnCollisionEnter2D(collision);
    }

    public void TriggerEnter2D(Collider2D childCollider, Collider2D collider)
    {

        if (dangerBox == childCollider)
        {
            var target = collider.GetComponent<Killable>();

            if (target && target.CombatTeam == Killable.Team.Heros)
            {
                enemyInRange = true;
            }
        }
    }

    public void TriggerExit2D(Collider2D childCollider, Collider2D collider)
    {
        if (dangerBox == childCollider)
        {
            var target = collider.GetComponent<Killable>();

            if (target && target.CombatTeam == Killable.Team.Heros)
            {
                enemyInRange = false;
                shotTime = 0;
            }
        }
    }
}
