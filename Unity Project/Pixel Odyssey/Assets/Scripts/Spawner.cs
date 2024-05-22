using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;
    void Start()
    {
        GameManager.Instance.updateGameGoal(numToSpawn); // Increment the enemy count
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numToSpawn) // If the spawner is set to start spawning, is not already spawning and the number of objects spawned is less than the number to spawn
        { 
            StartCoroutine(spawnObject()); // Start the coroutine to spawn the object
        }
    }


    private void OnTriggerEnter(Collider other) // Triggers when player enters the collider
    {
        if (other.CompareTag("Player")) // If the player enters the collider and the spawner is not already spawning
        {
            startSpawning = true; // Set the spawner to start spawning
        }
    }

    IEnumerator spawnObject() // Coroutine to spawn the object
    {

        int arrayPos = Random.Range(0, spawnPos.Length); // Get a random number between 0 and the number of spawn positions
        isSpawning = true; // Set the spawner to be spawning
        Instantiate(objectToSpawn, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation); // Instantiate the object at the random spawn position
        spawnCount++; // Increment the number of objects spawned
        yield return new WaitForSeconds(spawnTimer); // Wait for the spawn timer before spawning the next object
        isSpawning = false; // Set the spawner to not be spawning


    }
}

