using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveSet", menuName = "Combat/MoveSet")]
public class MoveSet : ScriptableObject
{
    [SerializeField]
    private List<Attack> moves = null;

    public Attack[] Moves { get; private set; }

    public void OnEnable()
    {
        if (moves != null)
        {
            Moves = moves.ToArray();
        }
    }
}
