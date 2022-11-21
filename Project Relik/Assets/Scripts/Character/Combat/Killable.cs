using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class Killable : MonoBehaviour
{
    public enum Team { None, Heros, Enemies }

    [SerializeField]
    private Team combatTeam = Team.None;
    [SerializeField]
    private float health = 3;
    [SerializeField]
    private bool isInvinsible = false;
    [SerializeField]
    private bool superArmorActive = false;
    [SerializeField]
    private UnityEvent m_MyEvent;

    private Animator animator = null;
    private new Rigidbody2D rigidbody = null;
    private List<GameObject> immuneList = null;

    public Team CombatTeam
    {
        get { return combatTeam; }
    }

    #region Unity Messages
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        immuneList = new List<GameObject>();
    }

    private void OnDestroy()
    {
        immuneList.RemoveRange(0, immuneList.Count);
    }
    #endregion

    #region Component Actions
    public void Push(float pushForce, Vector2 pushDirection)
    {
        rigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        Debug.Log(pushDirection * pushForce);
    }

    public void TakeDamage(float damage, VisualEffect impactEffect = null)
    {
        if (isInvinsible)
        {
            return;
        }

        health -= damage;

        if (impactEffect != null)
        {
            impactEffect.Play();
        }

        if (health <= 0)
        {
            animator.SetTrigger("Death");
        }
        else if(!superArmorActive)
        {
            animator.SetTrigger("Hurt");
        }
    }

    public bool IsImmune(GameObject obj)
    {
        return immuneList.Contains(obj);
    }

    public void AddImmuneObject(GameObject obj)
    {
        immuneList.Add(obj);
    }

    public void RemoveImmuneObject(GameObject obj)
    {
        immuneList.Remove(obj);
    }
    #endregion

    #region Animation Events
    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion
}
