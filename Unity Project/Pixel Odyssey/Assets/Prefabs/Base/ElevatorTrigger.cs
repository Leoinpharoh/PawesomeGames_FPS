using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{

    [SerializeField] public Animator animator;
    [SerializeField] GameObject triggerToDestroy;
    [SerializeField] GameObject triggerToActivate;
    private void OnTriggerEnter(Collider other) // Triggers when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider and the spawner is not already spawning
        {
            animator.SetBool("Trigger", true);
            triggerToActivate.SetActive(true);
            Destroy(triggerToDestroy);
        }
    }
}
