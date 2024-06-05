using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSceneAnimations : MonoBehaviour
{
    public Animator animator;

    // Function to be called by the animation event
    public void TriggerNextAnimation()
    {
        animator.SetTrigger("Next Animation");
    }
}
