using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "Narrative/Script")]
public class DialogueScript : ScriptableObject
{
    [SerializeField]
    private List<string> lines = null;

    public IReadOnlyList<string> Lines { get; private set; }

    public void OnEnable()
    {
        if (lines != null)
        {
            Lines = lines.AsReadOnly();
        }
    }
}

