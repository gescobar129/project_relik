using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClipBehavior : PlayableBehaviour
{
    public bool Visible { get; set; }
    public int DialogueLine { get; set; }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var dialogueWriter = playerData as DialogueWriter;

        if (dialogueWriter != null)
        {
            if (Visible)
            {
                dialogueWriter.ShowLine();
                dialogueWriter.SetDialogueLine(DialogueLine);
            }
            else
            {
                dialogueWriter.HideLine();
            }

        }
    }
}
