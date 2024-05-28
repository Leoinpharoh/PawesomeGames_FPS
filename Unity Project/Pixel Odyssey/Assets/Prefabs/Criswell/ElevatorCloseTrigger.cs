using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCloseTrigger : MonoBehaviour
{
    private Vector3 teleportPosition = new Vector3(-195.405f, 78.178f, -33.22f);
    private Vector3 teleportRotation = new Vector3(0f, 88.452f, 0f);
    [SerializeField] public Animator animator;
    private void OnTriggerEnter(Collider other) // Triggers when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider and the spawner is not already spawning
        {
            animator.SetTrigger("Close"); // Close the elevator door
            StartCoroutine(Televator()); // Start the Televator coroutine
        }
    }

    IEnumerator Televator()
    {
        yield return new WaitForSeconds(5); // Wait for 3 seconds
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject zombie in zombies)
        {
            Destroy(zombie);
        }
        yield return new WaitForSeconds(3); // Wait for 1 second
        GameObject.Find("Player").transform.position = teleportPosition; // Teleport the player to the teleportPosition
        GameObject.Find("Player").transform.eulerAngles = teleportRotation; // Rotate the player to the teleportRotation
    }
}
