using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MultiModelEnemyAI : MonoBehaviour, IDamage
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] SphereCollider triggerCollider;

    [SerializeField] EnemyParams enemyParams; // Reference to the EnemyParams ScriptableObject
    bool isAttacking = false; // Bool to check if the enemy is attacking
    bool playerInRange = false; // Bool to check if the player is in range of the enemy
    bool playerInMeleeAttackRange = false; // Bool to check if the player is in melee attack range of the enemy
    bool playerInRangedAttackRange = false; // Bool to check if the player is in ranged attack range of the enemy
    int HP; // Enemy Health
    float meleeRange; // Enemy Attack Range
    float rangedRange; // Enemy Attack Range
    Vector3 playerDir; // Vector3 to store the direction to the player
    Vector3 startingPos; // Vector3 to store the starting position of the enemy
    float angleToPlayer; // Float to store the angle to the player
    private Transform playerTransform; // Reference to the player's transform

    EnemyParams.EnemyType enemyType; // references the enemy type from the EnemyParams scriptable object
    EnemyParams.DetectionType enemyDetection; // references the enemy detection from the EnemyParams scriptable object

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.updateGameGoal(1);
        HP = enemyParams.HP; // Set the HP to the HP from the EnemyParams scriptable object
        enemyType = enemyParams.enemyType; // Get the enemy type from the EnemyParams scriptable object
        enemyDetection = enemyParams.detectionType; // Get the enemy detection from the EnemyParams scriptable object
        triggerCollider.radius = enemyParams.lineOfSightRange; // Set the radius of the trigger collider to the lineOfSightRange from the EnemyParams scriptable object
        triggerCollider.isTrigger = true; // Set the trigger collider to true
        agent.acceleration = enemyParams.Acceleration; // Set the acceleration of the NavMeshAgent to the Acceleration from the EnemyParams scriptable object
        agent.speed = enemyParams.movementSpeed; // Set the speed of the NavMeshAgent to the movementSpeed from the EnemyParams scriptable object
        meleeRange = enemyParams.meleeRange; // Set the meleeRange to the meleeRange from the EnemyParams scriptable object
        rangedRange = enemyParams.rangedRange; // Set the rangedRange to the rangedRange from the EnemyParams scriptable object
        playerTransform = GameManager.Instance.player.transform; // Get the player's transform from the GameManager
        agent.stoppingDistance = enemyParams.rangedRange; // Set the stopping distance of the NavMeshAgent to the rangedRange from the EnemyParams scriptable object
        if (enemyType == EnemyParams.EnemyType.Ranged) 
        {
            agent.stoppingDistance = enemyParams.rangedRange - 3; // Set the stopping distance of the NavMeshAgent to the rangedRange from the EnemyParams scriptable object
            if (agent.stoppingDistance < 1) // check if the stopping distance is less than 1
            {
                agent.stoppingDistance = 2; // Set the stopping distance to 1
            }
        }
        if (enemyType == EnemyParams.EnemyType.Melee || enemyType == EnemyParams.EnemyType.Combination) // Check if the enemy is a Melee or Combination enemy
        {
            agent.stoppingDistance = enemyParams.meleeRange-1; // Set the stopping distance of the NavMeshAgent to the meleeRange from the EnemyParams scriptable object
            if(agent.stoppingDistance < 1) // Check if the stopping distance is less than 1
            {
                agent.stoppingDistance = 1; // Set the stopping distance to 1
            }
        }




    }

    // Update is called once per frame
    void Update()
    {
//Wave Based Enemy=======================================================
        if (enemyDetection == EnemyParams.DetectionType.Wave)
        {
            agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
            if (!isAttacking && enemyType == EnemyParams.EnemyType.Ranged && playerInRangedAttackRange || (enemyType == EnemyParams.EnemyType.Combination && !playerInMeleeAttackRange)) // Check if the enemy is a Ranged enemy and is not shooting
            {
                StartCoroutine(shoot()); // Start the shoot coroutine
            }

            if(!isAttacking && (enemyType == EnemyParams.EnemyType.Melee || enemyType == EnemyParams.EnemyType.Combination)  && playerInMeleeAttackRange)
            {
                StartCoroutine(meleeAttack());
            }
        }
//Wave Based Enemy=======================================================

//Line of Sight Enemy=======================================================
        if (playerInRange && enemyType != EnemyParams.EnemyType.Stationary && enemyDetection != EnemyParams.DetectionType.Wave && canSeePlayer()) // Check if the player is in range and the enemy is not a Stationary enemy
        {
            if (enemyType == EnemyParams.EnemyType.Ranged)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player
                agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                if (!isAttacking && playerInRangedAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                {
                    StartCoroutine(shoot()); // Start the shoot coroutine
                }
            }

            if(enemyType == EnemyParams.EnemyType.Melee)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player
                agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                if (!isAttacking && playerInMeleeAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                {
                    StartCoroutine(meleeAttack()); // Start the shoot coroutine
                }

            }

            if (enemyType == EnemyParams.EnemyType.Combination)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player
                agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                if (!isAttacking && playerInMeleeAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                {
                    StartCoroutine(meleeAttack()); // Start the shoot coroutine
                }
                if (!isAttacking && playerInRangedAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                {
                    StartCoroutine(shoot()); // Start the shoot coroutine
                }
            }

        }
//Line of Sight Enemy=======================================================

//Stationary Enemy=======================================================
        if (playerInRange && enemyType == EnemyParams.EnemyType.Stationary)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player
            if (!isAttacking && playerInRangedAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
            {
                StartCoroutine(shoot()); // Start the shoot coroutine
            }
        }
//Stationary Enemy=======================================================
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            float distance = Vector3.Distance(transform.position, other.transform.position); // Get the distance between the enemy and the player

            if (enemyType == EnemyParams.EnemyType.Ranged)
            {
                if (distance <= rangedRange) // Check if the player is in attack range
                {
                    playerInRangedAttackRange = true; // Set playerInAttackRange to true
                }
                else
                {
                    playerInRangedAttackRange = false;
                }
            }

            if (enemyType == EnemyParams.EnemyType.Melee)
            {
                if (distance <= meleeRange) // Check if the player is in attack range
                {
                    playerInMeleeAttackRange = true; // Set playerInAttackRange to true
                }
                else
                {
                    playerInMeleeAttackRange = false;
                }
            }

            if (enemyType == EnemyParams.EnemyType.Combination)
            {
                if (distance <= meleeRange)
                {
                    playerInRangedAttackRange = false;
                    playerInMeleeAttackRange = true;
                }
                else if (distance <= rangedRange)
                {
                    playerInRangedAttackRange = true;
                    playerInMeleeAttackRange = false;
                }
                else
                {
                    playerInRangedAttackRange = false;
                    playerInMeleeAttackRange = false;
                }

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInRangedAttackRange = false; // Set playerInAttackRange to false
            playerInMeleeAttackRange = false; // Set playerInMeleeAttackRange to false
        }
    }

    IEnumerator shoot()
    {
        isAttacking = true;
        Instantiate(enemyParams.bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(enemyParams.attackSpeed);
        isAttacking = false;
    }

    public void takeDamage(int amount, Vector3 hitPosition) // Method to take damage
    {

        HP -= amount; // Subtract the amount from the enemy's health
        StartCoroutine(Damage(hitPosition)); // Start the flash coroutine
        if (HP <= 0) // Check if the enemy's health is less than or equal to 0
        {
            //GameManager.Instance.updateGameGoal(-1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one less enemy in the scene
            Destroy(gameObject); // Destroy the enemy
        }
        else
        {
            agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the agent to the player's position
        }
    }

    IEnumerator Damage(Vector3 hitPosition)
    {
        GameObject bloodEffect = Instantiate(enemyParams.bloodSplash, hitPosition, Quaternion.identity); // Instantiate at the hit position
        bloodEffect.transform.SetParent(transform); // Optionally set the parent to the enemy's transform
        yield return new WaitForSeconds(1); // Wait for 1 second (adjust based on your effect's needs)
        Destroy(bloodEffect); // Optionally destroy the effect after it finishes playing

    }

    IEnumerator meleeAttack() // Coroutine to handle the enemy's attack
    {
        if (isAttacking) // Check if the enemy is already attacking
        {
            yield break; // Prevent multiple simultaneous attacks
        }
        isAttacking = true; // Set isAttacking to true
        agent.isStopped = true; // Stop the agent from moving
        playerInRangedAttackRange = false; // Set playerInAttackRange to false
        playerInMeleeAttackRange = false; // Set playerInMeleeAttackRange to false

        PlayerManager playerHealth = GameManager.Instance.player.GetComponent<PlayerManager>();
        if (playerHealth != null) // Check if the playerHealth component is found on the player
        {
            playerHealth.takeDamage(enemyParams.meleeDamage, GameManager.Instance.player.transform.position); // Call the takeDamage function from the playerHealth script

            // Apply manual knockback
            Transform playerTransform = GameManager.Instance.player.transform;
            Vector3 knockbackDirection = (playerTransform.position - transform.position).normalized;
            float knockbackDistance = 2.0f; // Customize the distance as needed
            Vector3 newPlayerPosition = playerTransform.position + knockbackDirection * knockbackDistance;

            // Optionally use a coroutine to smoothly translate the player to the new position
            StartCoroutine(SmoothKnockback(playerTransform, newPlayerPosition, 0.2f));
        }

        yield return new WaitForSeconds(enemyParams.attackSpeed); // Wait while attack is ongoing

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



    bool canSeePlayer()
    {
        playerDir = GameManager.Instance.player.transform.position - headPos.position; // Get the direction to the player
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward); // Get the angle to the player

        RaycastHit hit; // Create a raycast hit variable
        if (Physics.Raycast(headPos.position, playerDir, out hit)) // Check if the raycast hits something
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= enemyParams.viewAngle && playerInRange) // Check if the object hit is the player
            {
                agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the agent to the player's position
                
                return true; // Return true
            }
        }
        return false; // Return false
    }
}