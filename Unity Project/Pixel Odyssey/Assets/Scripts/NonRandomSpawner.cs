using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn; // Array of objects to spawn
    [SerializeField] private int numToSpawn; // Number of objects to spawn
    [SerializeField] private float spawnTimer; // Time between spawns
    [SerializeField] private Transform[] spawnPos; // Array of spawn positions

    private int spawnCount; // Counter for number of objects spawned
    private bool isSpawning; // Is the spawner currently spawning
    private bool startSpawning; // Should the spawner start spawning

    private void Start()
    {
        //GameManager.Instance.updateGameGoal(numToSpawn); // Increment the enemy count
    }

    private void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numToSpawn) // If the spawner should start, isn't spawning, and hasn't spawned enough objects
        {
            StartCoroutine(SpawnObject()); // Start spawning objects
        }
    }

    private void OnTriggerEnter(Collider other) // Trigger when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider
        {
            startSpawning = true; // Set the spawner to start spawning
        }
    }
    private void OnTriggerStay(Collider other) // Trigger when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider
        {
            startSpawning = true; // Set the spawner to start spawning
        }
    }

    private IEnumerator SpawnObject() // Coroutine to spawn objects
    {
        isSpawning = true; // Set the spawner to be spawning

        while (spawnCount < numToSpawn) // While there are still objects to spawn
        {
            for(int i = 0; i < objectsToSpawn.Length; i++)
            {
                int arrayPos = spawnCount % spawnPos.Length; // Get the current spawn position
                Instantiate(objectsToSpawn[i], spawnPos[arrayPos].position, spawnPos[arrayPos].rotation); // Instantiate the object at the current spawn position
                spawnCount++; // Increment the number of objects spawned
                yield return new WaitForSeconds(spawnTimer); // Wait for the spawn timer before spawning the next object
            }
        }

        isSpawning = false; // Set the spawner to not be spawning
    }
}
