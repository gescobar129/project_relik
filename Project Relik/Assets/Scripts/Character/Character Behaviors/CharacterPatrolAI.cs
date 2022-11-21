using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPatrolAI : MonoBehaviour
{
    [SerializeField]
    private float walkTimeLength = 1;

    protected float walkTime = 0;

    private CharacterMovement characterMovement = null;
    private bool lastGroundCheck = false;

    protected void AIUnityAwake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    // Start is called before the first frame update
    protected void AIUnityStart()
    {
        characterMovement.Run(CharacterMovement.Direction.Right);
    }

    protected void AIUnityUpdate()
    {
        walkTime += Time.deltaTime;

        if (walkTime >= walkTimeLength)
        {
            ToggleDirection();
        }

        if (!characterMovement.IsOnGround)
        {
            lastGroundCheck = false;
        }
    }

    public void AIUnityOnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Ground" || lastGroundCheck)
        {
            ToggleDirection();
        }

        if (characterMovement.IsOnGround)
        {
            lastGroundCheck = true;
        }
    }

    protected void ToggleDirection()
    {
        CharacterMovement.Direction currentDirection = CharacterMovement.Direction.None;

        if (characterMovement.HorizontalMovement == CharacterMovement.Direction.Right)
        {
            currentDirection = CharacterMovement.Direction.Left;
        }
        else if (characterMovement.HorizontalMovement == CharacterMovement.Direction.Left)
        {
            currentDirection = CharacterMovement.Direction.Right;
        }

        walkTime = 0;
        characterMovement.Run(currentDirection);
    }
}
