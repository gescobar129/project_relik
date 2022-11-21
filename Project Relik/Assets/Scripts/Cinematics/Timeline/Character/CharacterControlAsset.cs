using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CharacterControlAsset : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private CharacterMovement.Direction runDirection = CharacterMovement.Direction.None;

    [SerializeField]
    private bool disableMovement;

	public ClipCaps clipCaps
	{
		get { return ClipCaps.None; }
	}

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CharacterControlBehaviour>.Create(graph);

        var characterMovement = playable.GetBehaviour();
        characterMovement.RunDirection = runDirection;
        characterMovement.DisableMovement = disableMovement;

        return playable;
    }
}
