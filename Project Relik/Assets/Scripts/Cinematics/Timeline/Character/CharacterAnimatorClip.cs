using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CharacterAnimatorClip : PlayableAsset, ITimelineClipAsset
{
    public enum Parameter { BOOL, TRIGGER }

    [SerializeField]
    private List<ParameterValue> animationParameters;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CharacterAnimatorBehavior>.Create(graph);

        var characterAnimator = playable.GetBehaviour();
        characterAnimator.AnimationParameters = animationParameters;

        return playable;
    }

    [Serializable]
    public struct ParameterValue
    {
        public Parameter type;
        public string name;
        public string value;
    }
}
