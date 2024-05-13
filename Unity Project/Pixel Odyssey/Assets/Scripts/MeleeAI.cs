using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour, IDamage
{

    [SerializeField] Renderer model;
    [SerializeField] int HP;
    [SerializeField] float attackSpeed;
    [SerializeField] NavMeshAgent agent; // Will allow the enemy to move around the map

    [SerializeField] private Collider followCollider; // Will cause enemy to follow player when in range
    [SerializeField] private Collider attackCollider; // Will cause enemy to attack player when in range

    [SerializeField] private GameObject bloodSplash; // Creates a reference to the blood splash

    bool isAttacking; // Bool to check if the enemy is attacking
    bool playerInRange; // Bool to check if the player is in range of the enemy 
    bool playerInAttackRange; // Bool to check if the player is in attack range of the enemy
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange) // Check if the player is in range of the enemy
        {
            agent.SetDestination(GameManager.instance.player.transform.position); // Set the destination of the agent to the player's position
        }

        if (playerInAttackRange) // Check if the player is in attack range
        {
            if (!isAttacking) // Check if the enemy is not attacking
            {
                StartCoroutine(attack()); // Start the attack coroutine
            }
        }

    }

    void OnTriggerEnter(Collider other) // Check if the player is in range of the enemy
    {
        if (other.gameObject == followCollider.gameObject && other.CompareTag("Player")) // Checks if the player is in range of the enemy to be seen
        {
            playerInRange = true; // Set playerInRange to true
        }
        else if (other.gameObject == attackCollider.gameObject && other.CompareTag("Player")) // Checks to see if the player is in range of the enemy to be attacked
        {
            playerInAttackRange = true; // Set playerInAttackRange to true
        }
    }

    void OnTriggerExit(Collider other) // Check if the player is out of range of the enemy
    { 
        if (other.gameObject == followCollider.gameObject && other.CompareTag("Player")) // Checks if the player is out of range of the enemy to be seen
        {
            playerInRange = false; // Set playerInRange to false
        }
        else if (other.gameObject == attackCollider.gameObject && other.CompareTag("Player")) // Checks if the player is out of range of the enemy to be attacked
        {
            playerInAttackRange = false; // Set playerInAttackRange to false
        }
    }


    public void takeDamage(int amount, Vector3 hitPosition) // Method to take damage
    {
        HP -= amount; // Subtract the amount from the enemy's health
        StartCoroutine(Damage(hitPosition)); // Start the flash coroutine
        if (HP <= 0) // Check if the enemy's health is less than or equal to 0
        {
            Destroy(gameObject); // Destroy the enemy
        }
    }

    IEnumerator Damage(Vector3 hitPosition)
    {
        GameObject bloodEffect = Instantiate(bloodSplash, hitPosition, Quaternion.identity); // Instantiate at the hit position
        bloodEffect.transform.SetParent(transform); // Optionally set the parent to the enemy's transform
        yield return new WaitForSeconds(1); // Wait for 1 second (adjust based on your effect's needs)
        Destroy(bloodEffect); // Optionally destroy the effect after it finishes playing
    }

    IEnumerator attack()
    {
        isAttacking = true; // Set isAttacking to true
        yield return new WaitForSeconds(attackSpeed); // Wait for the attack speed
        isAttacking = false; // Set isAttacking to false
    }

}
