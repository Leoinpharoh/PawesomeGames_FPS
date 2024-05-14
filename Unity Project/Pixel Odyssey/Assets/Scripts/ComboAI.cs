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
    [SerializeField] float meleeRange; // Enemy Attack Range
    [SerializeField] int damage; // Damage the enemy will deal to the player

    [SerializeField] private Collider followCollider; // Will cause enemy to follow player when in range
    [SerializeField] private Collider attackCollider; // Will cause enemy to attack player when in range

    [SerializeField] private GameObject bloodSplash; // Creates a reference to the blood splash

    bool isAttacking; // Bool to check if the enemy is attacking
    bool playerInRange; // Bool to check if the player is in range of the enemy 
    bool playerInAttackRange; // Bool to check if the player is in attack range of the enemy
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.updateGameGoal(1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one more enemy in the scene
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange) // Check if the player is in range of the enemy
        {
            agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the agent to the player's position
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
        if (other.CompareTag("Player")) // Check if the other object is the player
        {
            if (Vector3.Distance(transform.position, other.transform.position) <= meleeRange)
            {
                playerInAttackRange = true;  // Player entered melee range
                Debug.Log("Player in attack range");
            }
            else
            {
                playerInRange = true;  // Player is in detection range but not in melee range
                Debug.Log("Player in range");
            }
        }
    }

    void OnTriggerExit(Collider other) // Check if the player is out of range of the enemy
    {
        if (other.CompareTag("Player")) // Check if the other object is the player
        {
            if(Vector3.Distance(transform.position, other.transform.position) <= meleeRange)
            {
                playerInRange = true; // Player exited melee range
                Debug.Log("Player in range");
                StartCoroutine(attack());
            }
            else
            {
                playerInRange = false; // Player is out of detection range
                Debug.Log("Player out of range");
            }
        }
    }


    public void takeDamage(int amount, Vector3 hitPosition) // Method to take damage
    {
        HP -= amount; // Subtract the amount from the enemy's health
        StartCoroutine(Damage(hitPosition)); // Start the flash coroutine
        if (HP <= 0) // Check if the enemy's health is less than or equal to 0
        {
            GameManager.Instance.updateGameGoal(-1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one less enemy in the scene
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
        if (isAttacking)
        {
            yield break; // Prevent multiple simultaneous attacks
        }
        isAttacking = true; // Set isAttacking to true
        Debug.Log("Attacking"); // Log that the enemy is attacking
        agent.isStopped = true; // Stop the agent from moving
        playerInAttackRange = false;

        PlayerManager playerHealth = GameManager.Instance.player.GetComponent<PlayerManager>();
        if (playerHealth != null) // Check if the playerHealth component is found on the player
        {
            playerHealth.takeDamage(damage, GameManager.Instance.player.transform.position); // Call the takeDamage function from the playerHealth script

            // Apply manual knockback
            Transform playerTransform = GameManager.Instance.player.transform;
            Vector3 knockbackDirection = (playerTransform.position - transform.position).normalized;
            float knockbackDistance = 2.0f; // Customize the distance as needed
            Vector3 newPlayerPosition = playerTransform.position + knockbackDirection * knockbackDistance;

            // Optionally use a coroutine to smoothly translate the player to the new position
            StartCoroutine(SmoothKnockback(playerTransform, newPlayerPosition, 0.2f));
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on the player."); // Log an error if the playerHealth component is not found
        }
        
        yield return new WaitForSeconds(attackSpeed); // Wait while attack is ongoing

        agent.isStopped = false; // Re-enable movement
        Debug.Log("Attack Complete"); // Log that the attack is complete
        isAttacking = false; // Set isAttacking to false
        
    }

    IEnumerator SmoothKnockback(Transform playerTransform, Vector3 targetPosition, float duration) // Coroutine to smoothly knockback the player
    {
        float time = 0; // Initialize time to 0
        Vector3 startPosition = playerTransform.position; // Get the player's current position
        while (time < duration) // Loop while time is less than duration
        {
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, time / duration); // Smoothly move the player towards the target position
            time += Time.deltaTime; // Increment time by Time.deltaTime
            yield return null; // Wait for the next frame
        }
        playerTransform.position = targetPosition; // Ensure the player reaches the target position
    }



}
