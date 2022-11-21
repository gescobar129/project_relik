using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CharacterControlBehaviour : PlayableBehaviour
{
    public CharacterMovement.Direction RunDirection { get; set; }
    public bool DisableMovement { get; set; }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var characterMovement = playerData as CharacterMovement;

        if (DisableMovement && characterMovement.enabled)
        {
            characterMovement.enabled = false;
        }
        else
        {
            if (!characterMovement.enabled)
            {
                characterMovement.enabled = true;
            }

            if (characterMovement != null)
            {
                characterMovement.Run(RunDirection);
            }
        }
    }
}
