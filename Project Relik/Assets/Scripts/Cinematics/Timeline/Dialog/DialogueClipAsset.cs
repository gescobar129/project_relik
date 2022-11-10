using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueClipAsset : PlayableAsset, ITimelineClipAsset
{
	[SerializeField]
	private int dialogueLine = 0;
	[SerializeField]
	private bool visible = true;

	public ClipCaps clipCaps
	{
		get { return ClipCaps.None; }
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		var playable = ScriptPlayable<DialogueClipBehavior>.Create(graph);

		var dialogueBehavior = playable.GetBehaviour();
		dialogueBehavior.DialogueLine = dialogueLine;
		dialogueBehavior.Visible = visible;

		return playable;
	}
}
