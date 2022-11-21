using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterCutScene : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement.Direction walkDirection = CharacterMovement.Direction.None;
    [SerializeField]
    private string currentAnimation = "";

    private CharacterMovement character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAnimation == "")
        {
            character.Run(walkDirection);
        }
    }
}
