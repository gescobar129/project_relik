using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour, ICollisionEventTarget
{
    private static readonly float JumpTime = 0.2f;
    public enum Direction { Left = 1, Right = 2, Down = 3, Up = 4, None = 0 }

    [SerializeField]
    private float groundMovementSpeed = 0;
    [SerializeField]
    private float arialMovementSpeed = 0;
    [SerializeField]
    private float maxSpeed = 0;
    [SerializeField]
    private float maxAirSpeed = 20;
    [SerializeField]
    private float jumpForce = 0;
    [SerializeField]
    private float groundDrag = 0;
    [SerializeField]
    private float arialDrag = 0;
    [SerializeField]
    private Collider2D groundTrigger = null;
    [SerializeField]
    private bool freezeDirection = false;

    private Animator animator;
    private Rigidbody2D rb2d;
    private Collider2D[] possibleGroundTriggers = null;
    private Transform focusPoint = null;
    private Direction horizontalMovement;
    private Direction faceDirection;
    private bool isOnGround = true;
    private bool isJumping = false;
    private float jumpTimer = 0;

    public Direction HorizontalMovement
    {
        get { return horizontalMovement; }
    }

    public Direction CurrentDirection
    {
        get { return faceDirection; }
    }

    public bool IsOnGround
    {
        get { return isOnGround; }
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Collider2D[] colliders;
        colliders = GetComponents<BoxCollider2D>();

        if (!groundTrigger)
        {
            groundTrigger = GetComponent<BoxCollider2D>();
        }

        if (!focusPoint)
        {
            focusPoint = transform;
        }

        horizontalMovement = Direction.None;
        faceDirection = Direction.Right;

        possibleGroundTriggers = new Collider2D[2];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void LateUpdate()
    {
        rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb2d.velocity.y, -maxAirSpeed, maxAirSpeed));
    }

    private void FixedUpdate()
    {
        float movementSpeed;
        if (isOnGround)
        {
            movementSpeed = groundMovementSpeed;
        }
        else
        {
            movementSpeed = arialMovementSpeed;
        }

        if (horizontalMovement != Direction.None && isOnGround)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (!freezeDirection)
        {
            if (horizontalMovement == Direction.Right)
            {
                rb2d.AddForce(new Vector2(movementSpeed * Time.deltaTime, 0), ForceMode2D.Impulse);
            }
            else if (horizontalMovement == Direction.Left)
            {
                rb2d.AddForce(new Vector2(-movementSpeed * Time.deltaTime, 0), ForceMode2D.Impulse);
            }

            UpdateRotation();
        }

        if (jumpTimer >= JumpTime)
        {
            StopJump();
        }

        if (/*isOnGround &&*/ isJumping)
        {
            jumpTimer += Time.deltaTime;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        if (isOnGround)
        {
            rb2d.drag = groundDrag;
        }
        else
        {
            rb2d.drag = arialDrag;
        }

    }

    private void UpdateRotation()
    {
        Vector3 rotation = rb2d.transform.localRotation.eulerAngles;
        if (faceDirection != DetectFaceDirection())
        {
            rb2d.transform.Rotate(0f, 180f, 0f);
        }
    }

    public void Run(Direction direction)
    {
        horizontalMovement = direction;
        if (horizontalMovement == Direction.Right)
        {
            if (faceDirection == Direction.Left)
            {
                faceDirection = Direction.Right;
            }
        }
        else if (horizontalMovement == Direction.Left)
        {
            if (faceDirection == Direction.Right)
            {
                faceDirection = Direction.Left;
            }
        }

        if (!freezeDirection)
        {
            UpdateRotation();
        }
    }

    public void FaceDirection(Direction direction)
    {
        Run(direction);
        Idle();
    }

    public Direction DetectFaceDirection()
    {
        Vector3 rotation = rb2d.transform.localRotation.eulerAngles;

        float y = rotation.y;

        while (y > 360f)
        {
            y -= 360f;
        }

        while (y < 0)
        {
            y += 360;
        }

        if (y < 90f || y > 270f)
        {
            return Direction.Right;
        }
        else if (y >= 90f && y <= 270f)
        {
            return Direction.Left;
        }

        return Direction.Right;
    }

    public void ToggleDirection()
    {
        if (faceDirection == Direction.Left)
        {
            FaceDirection(Direction.Right);
        }
        else if (faceDirection == Direction.Right)
        {
            FaceDirection(Direction.Left);
        }
    }

    public void Idle()
    {
        horizontalMovement = Direction.None;
    }

    public void Jump()
    {
        if (isOnGround)
        {
            isJumping = true;
        }
    }

    public void StopJump()
    {
        isJumping = false;
        jumpTimer = 0;
    }

    public void TriggerEnter2D(Collider2D childCollider, Collider2D collider)
    {
        if (childCollider == groundTrigger)
        {
            if (collider.tag == "Ground")
            {
                isOnGround = true;
            }
        }
    }

    public void TriggerExit2D(Collider2D childCollider, Collider2D collider)
    {
        if (childCollider == groundTrigger)
        {
            if (collider.tag == "Ground")
            {
                for (int i = 0; i < possibleGroundTriggers.Length; i++)
                {
                    possibleGroundTriggers[i] = null;
                }

                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(LayerMask.GetMask("Ground"));
                groundTrigger.OverlapCollider(filter, possibleGroundTriggers);

                //If we got any result then we are in the ground
                isOnGround = possibleGroundTriggers[0];
            }
        }
    }
}
