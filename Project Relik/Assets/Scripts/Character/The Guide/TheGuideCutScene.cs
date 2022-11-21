using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGuideCutScene : MonoBehaviour
{
    private enum TheGuideAnimation { None = 0, Teleport = 1 };

    [SerializeField]
    private TheGuideAnimation currentAnimation = TheGuideAnimation.None;

    private Animator animator = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAnimation != TheGuideAnimation.None)
        {
            switch(currentAnimation)
            {
                case TheGuideAnimation.Teleport:
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("teleport"))
                    {
                        animator.SetTrigger("Teleport");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
