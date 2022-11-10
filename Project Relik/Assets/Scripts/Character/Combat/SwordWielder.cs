using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWielder : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = null;
    [SerializeField]
    private Weapon weapon = null;
    [SerializeField]
    private Transform focusPoint;
    [SerializeField]
    private bool canAttack = true;

    private Killable healthComponent = null;

    public Weapon CurrentWeapon
    {
        get { return weapon; }
        set
        {
            if (weapon && healthComponent)
            {
                healthComponent.RemoveImmuneObject(weapon.gameObject);
            }

            weapon = value;
            UpdateImmuneObject();
        }
    }

    private void Awake()
    {
        focusPoint = transform.GetChild(0);

        if (!focusPoint)
        {
            focusPoint = transform;
        }

        healthComponent = GetComponent<Killable>();
    }

    void Start()
    {
        UpdateImmuneObject();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CurrentWeapon)
        {
            CurrentWeapon.MoveToPoint(focusPoint.position);
        }
    }

    private void UpdateImmuneObject()
    {
        if (weapon && healthComponent)
        {
            healthComponent.AddImmuneObject(weapon.gameObject);
        }
    }

    public void AimWeapon(Vector2 aimTarget)
    {
        if (mainCamera && CurrentWeapon)
        {
            Vector3 screenVector = mainCamera.WorldToScreenPoint(focusPoint.position);
            Vector2 targetVector = aimTarget - new Vector2(screenVector.x, screenVector.y);

            float angle = 180 / Mathf.PI * Mathf.Atan2(targetVector.y, targetVector.x) - 90;
            CurrentWeapon.WeaponAngle = angle;
        }
    }

    public void Attack()
    {
        if (CurrentWeapon && canAttack)
        {
            CurrentWeapon.Attack();
        }
    }
}
