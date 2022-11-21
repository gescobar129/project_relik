using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CharacterAnimatorBehavior : PlayableBehaviour
{
    public List<CharacterAnimatorClip.ParameterValue> AnimationParameters { get; set; }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var characterMovement = playerData as CharacterMovement;

        if (characterMovement != null && AnimationParameters != null)
        {
            var animator = characterMovement.GetComponent<Animator>();

            foreach (CharacterAnimatorClip.ParameterValue param in AnimationParameters)
            {
                switch(param.type)
                {
                    case CharacterAnimatorClip.Parameter.BOOL:
                        if (Boolean.TryParse(param.value, out bool value))
                        {
                            animator.SetBool(param.name, value);
                        }
                        break;
                    case CharacterAnimatorClip.Parameter.TRIGGER:
                        animator.SetTrigger(param.name);
                        break;
                }
            }
        }
    }
}
