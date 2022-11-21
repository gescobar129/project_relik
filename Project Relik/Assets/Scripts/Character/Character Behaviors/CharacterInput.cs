using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static CharacterMovement;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterInput : MonoBehaviour
{
    private CharacterControls controls;
    private CharacterMovement character;
    private SwordWielder swordWielder;
    private CharacterCombat characterCombat;
    private MagicUser magicUser;
    private Vector2 mousePos;
    private CharacterMovement.Direction currentDirection = Direction.None;

    private void Awake()
    {
        controls = new CharacterControls();
        controls.Player.Movement.performed += HandleMovement;
        controls.Player.Movement.canceled += HandleMovement;
        controls.Player.Attack.performed += HandleAttack;
        controls.Player.Aim.performed += HandleAim;
        controls.Player.SonicButton.performed += FocusSonic;
        controls.Player.SonicButton.canceled += CastSonic;

        character = GetComponent<CharacterMovement>();
        swordWielder = GetComponent<SwordWielder>();
        characterCombat = GetComponent<CharacterCombat>();
        magicUser = GetComponent<MagicUser>();

    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (swordWielder)
        {
            swordWielder.AimWeapon(mousePos);
        }
        character.Run(currentDirection);
    }

    private void HandleMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        if (inputVector.x > 0.1f)
        {
            currentDirection = Direction.Right;
            //character.Run();
        }
        else if (inputVector.x < -0.1f)
        {
            currentDirection = Direction.Left;
            //character.Run(Direction.Left);
        }
        else
        {
            currentDirection = Direction.None;
            //character.Run(Direction.None);
        }

        if (inputVector.y > 0.1f)
        {
            character.Jump();
        }
        else if (inputVector.y <= 0.1f && inputVector.y > -0.1f)
        {
            character.StopJump();
        }
    }

    private void HandleAttack(InputAction.CallbackContext context)
    {
        if (swordWielder)
        {
            swordWielder.Attack();
        }

        if (characterCombat)
        {
            characterCombat.Attack(0);
        }
    }

    private void HandleAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void FocusSonic(InputAction.CallbackContext context)
    {
        if (magicUser == null)
        {
            return;
        }

        magicUser.FocusSpell(0);
    }

    private void CastSonic(InputAction.CallbackContext context)
    {
        if (magicUser == null)
        {
            return;
        }

        magicUser.CastMagic();
    }
}
