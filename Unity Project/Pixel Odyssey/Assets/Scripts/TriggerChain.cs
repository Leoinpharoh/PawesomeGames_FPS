using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChain : MonoBehaviour
{
    [SerializeField] GameObject triggerToDestroy;
    private void OnTriggerEnter(Collider other) // Triggers when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider and the spawner is not already spawning
        {
            Destroy(triggerToDestroy);
        }
    }
}
