using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(CharacterControlAsset))]
[TrackClipType(typeof(CharacterAnimatorClip))]
[TrackBindingType(typeof(CharacterMovement))]
public class CharacterControlTrack : TrackAsset
{}
