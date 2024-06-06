using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] public Animator animator;


    public void PauseAnimation()
    {
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    public void ResumeAnimation()
    {
        if (animator != null)
        {
            animator.speed = 1;
        }
    }
}