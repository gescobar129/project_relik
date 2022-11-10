using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBehavior : StateMachineBehaviour
{
    private GameObject effectObject = null;
    private Rigidbody2D rigidBody = null;
    private SwordWielder swordWielder = null;
    private CharacterMovement characterMovement = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidBody = animator.GetComponent<Rigidbody2D>();
        swordWielder = animator.GetComponent<SwordWielder>();
        characterMovement = animator.GetComponent<CharacterMovement>();

        var magicUser = animator.GetComponent<MagicUser>();
        effectObject = Instantiate(magicUser.ActiveSpell.EffectAssets[0], magicUser.EffectCenter);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (swordWielder == null)
        {
            return;
        }

        float targetAngle = (swordWielder.CurrentWeapon.WeaponAngle + 90) * (Mathf.PI/180);

        var targetVector = new Vector2(Mathf.Cos(targetAngle), Mathf.Sin(targetAngle));

        rigidBody.AddForce(targetVector, ForceMode2D.Impulse);

        if (targetVector.x > 0)
        {
            if (characterMovement.DetectFaceDirection() == CharacterMovement.Direction.Left)
            {
                rigidBody.transform.Rotate(0f, 180f, 0f);
                characterMovement.FaceDirection(CharacterMovement.Direction.Right);
            }
        }
        else if (targetVector.x < 0)
        {
            if (characterMovement.DetectFaceDirection() == CharacterMovement.Direction.Right)
            {
                rigidBody.transform.Rotate(0f, 180f, 0f);
                characterMovement.FaceDirection(CharacterMovement.Direction.Left);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(effectObject);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
