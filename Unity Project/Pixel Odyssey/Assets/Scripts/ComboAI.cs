using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ComboAI : MonoBehaviour, IDamage
{
    [SerializeField] private Renderer model; // Reference to the enemy's model
    [SerializeField] private Transform shootPos; // Reference to the position the enemy will shoot from

    [SerializeField] private int HP; // Enemy Health
    [SerializeField] private GameObject bullet; // Reference to the bullet prefab
    [SerializeField] private float shootRate; // Rate at which the enemy will shoot
    [SerializeField] float attackSpeed; // Attack speed of the enemy
    [SerializeField] private GameObject bloodSplash; // Creates a reference to the blood splash
    [SerializeField] float meleeRange; // Enemy Attack Range
    [SerializeField] int meleeDamage; // Damage the enemy will deal to the player
    [SerializeField] private float shootRange;

    private bool isShooting; // Bool to check if the enemy is shooting
    private bool playerInRange; // Bool to check if the player is in range of the enemy
    private bool playerInMeleeAttackRange; // Bool to check if the player is in melee attack range of the enemy
    private bool playerInAttackRange; // Bool to check if the player is in attack range of the enemy
    private bool isAttacking; // Bool to check if the enemy is attacking

    private Transform playerTransform; // Reference to the player's transform
    private NavMeshAgent agent; // Will allow the enemy to move around the map

    void Start()
    {
        GameManager.Instance.updateGameGoal(1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one more enemy in the scene
        playerTransform = GameManager.Instance.player.transform; // Set the playerTransform to the player's transform
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component on the enemy
    }

    void Update()
    {
        if (playerInRange) // Check if the player is in range of enemy
        {
            agent.SetDestination(playerTransform.position); // Set the destination of the agent to the player's position

            if (playerInAttackRange && !isShooting && !isAttacking) // Check if the player is in attack range and the enemy is not shooting or attacking
            {
                StartCoroutine(shoot()); // Start the shoot coroutine
            }

            if (playerInMeleeAttackRange) // Check if the player is in melee attack range
            {
                StartCoroutine(attack()); // Start the attack coroutine
            }

            Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Get the look rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate the enemy to face the player
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the other object is the player
        {
            playerInRange = true; // Set playerInRange to true
        }
    }

    private void OnTriggerStay(Collider other) // Check if the player is in range of the enemy
    {
        if (other.CompareTag("Player")) // Check if the other object is the player
        {
            float distance = Vector3.Distance(transform.position, other.transform.position); // Get the distance between the enemy and the player

            if (distance <= meleeRange) // Check if the player is in melee attack range
            {
                playerInMeleeAttackRange = true; // Set playerInMeleeAttackRange to true
                playerInAttackRange = false; // Set playerInAttackRange to false
            }
            else if (distance <= shootRange) // Check if the player is in attack range
            {
                playerInAttackRange = true; // Set playerInAttackRange to true
                playerInMeleeAttackRange = false; // Set playerInMeleeAttackRange to false
            }
            else
            {
                playerInAttackRange = false; // Set playerInAttackRange to false
                playerInMeleeAttackRange = false; // Set playerInMeleeAttackRange to false
            }
        }
    }

    private void OnTriggerExit(Collider other)  // Check if the player is out of range of the enemy
    {
        if (other.CompareTag("Player")) // Check if the other object is the player
        {
            playerInRange = false; // Set playerInRange to false
            playerInAttackRange = false; // Set playerInAttackRange to false
            playerInMeleeAttackRange = false; // Set playerInMeleeAttackRange to false
        }
    }

    IEnumerator shoot() // Coroutine to handle the enemy's shooting
    {
        isShooting = true; // Set isShooting to true
        Instantiate(bullet, shootPos.position, transform.rotation); // Instantiate the bullet at the shootPos position

        yield return new WaitForSeconds(shootRate); // Wait for the shootRate
        isShooting = false; // Set isShooting to false
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

    IEnumerator Damage(Vector3 hitPosition) // Coroutine to handle the damage effect
    {
        GameObject bloodEffect = Instantiate(bloodSplash, hitPosition, Quaternion.identity); // Instantiate at the hit position
        bloodEffect.transform.SetParent(transform); // Optionally set the parent to the enemy's transform
        yield return new WaitForSeconds(1); // Wait for 1 second (adjust based on your effect's needs)
        Destroy(bloodEffect); // Optionally destroy the effect after it finishes playing

    }

    IEnumerator attack() // Coroutine to handle the enemy's attack
    {
        if (isAttacking) // Check if the enemy is already attacking
        {
            yield break; // Prevent multiple simultaneous attacks
        }
        isAttacking = true; // Set isAttacking to true
        agent.isStopped = true; // Stop the agent from moving
        playerInAttackRange = false; // Set playerInAttackRange to false
        playerInMeleeAttackRange = false; // Set playerInMeleeAttackRange to false

        PlayerManager playerHealth = GameManager.Instance.player.GetComponent<PlayerManager>();
        if (playerHealth != null) // Check if the playerHealth component is found on the player
        {
            playerHealth.takeDamage(meleeDamage, GameManager.Instance.player.transform.position); // Call the takeDamage function from the playerHealth script

            // Apply manual knockback
            Transform playerTransform = GameManager.Instance.player.transform;
            Vector3 knockbackDirection = (playerTransform.position - transform.position).normalized;
            float knockbackDistance = 2.0f; // Customize the distance as needed
            Vector3 newPlayerPosition = playerTransform.position + knockbackDirection * knockbackDistance;

            // Optionally use a coroutine to smoothly translate the player to the new position
            StartCoroutine(SmoothKnockback(playerTransform, newPlayerPosition, 0.2f));
        }

        yield return new WaitForSeconds(attackSpeed); // Wait while attack is ongoing

        agent.isStopped = false; // Re-enable movement
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
