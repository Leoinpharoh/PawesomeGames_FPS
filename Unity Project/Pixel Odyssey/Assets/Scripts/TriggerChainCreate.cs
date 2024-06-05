using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChainCreate : MonoBehaviour
{
    [SerializeField] GameObject objectToCreate;
    private void OnTriggerEnter(Collider other) // Triggers when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider and the spawner is not already spawning
        {
            objectToCreate.SetActive(true);
        }
    }
}
