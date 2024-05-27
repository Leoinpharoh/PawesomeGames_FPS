using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] SphereCollider triggerCollider;
    [SerializeField] Animator anim;
    [SerializeField] EnemyParams enemyParams; // Reference to the EnemyParams ScriptableObject
    bool isAttacking = false; // Bool to check if the enemy is attacking
    bool playerInRange = false; // Bool to check if the player is in range of the enemy
    bool playerInMeleeAttackRange = false; // Bool to check if the player is in melee attack range of the enemy
    bool playerInRangedAttackRange = false; // Bool to check if the player is in ranged attack range of the enemy
    bool roaming = false; // Bool to check if the enemy is roaming
    bool destChosen;
    int HP; // Enemy Health
    int stoppingDistanceOriginal; // Original stopping distance of the NavMeshAgent
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
        HP = enemyParams.HP; // Set the HP to the HP from the EnemyParams scriptable object
        enemyType = enemyParams.enemyType; // Get the enemy type from the EnemyParams scriptable object
        enemyDetection = enemyParams.detectionType; // Get the enemy detection from the EnemyParams scriptable object
        triggerCollider.radius = enemyParams.lineOfSightRange; // Set the radius of the trigger collider to the lineOfSightRange from the EnemyParams scriptable object
        triggerCollider.isTrigger = true; // Set the trigger collider to true
        roaming = enemyParams.roaming; // Set the roaming bool to the roaming bool from the EnemyParams scriptable object
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
        stoppingDistanceOriginal = (int)agent.stoppingDistance; // Set the stoppingDistanceOriginal to the stopping distance of the NavMeshAgent
    }

    // Update is called once per frame
    void Update()
    {
        if(HP > 0) {
            float animSpeed = agent.velocity.normalized.magnitude; // Get the speed of the agent
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * enemyParams.animSpeedTrans)); // Set the speed of the animator
            if (animSpeed == 0 && canSeePlayer() == true)
            {
                anim.SetBool("isStopped", true);
            }
            else
            {
                anim.SetBool("isStopped", false);
            }
            //Wave Based Enemy=======================================================
            if (enemyDetection == EnemyParams.DetectionType.Wave)
            {
                agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                if (!isAttacking && enemyType == EnemyParams.EnemyType.Ranged && playerInRangedAttackRange || (enemyType == EnemyParams.EnemyType.Combination && !playerInMeleeAttackRange)) // Check if the enemy is a Ranged enemy and is not shooting
                {
                    StartCoroutine(shoot()); // Start the shoot coroutine
                }

                if (!isAttacking && (enemyType == EnemyParams.EnemyType.Melee || enemyType == EnemyParams.EnemyType.Combination) && playerInMeleeAttackRange)
                {
                    Debug.Log("Melee Attack");
                    StartCoroutine(meleeAttack());
                }
            }
            //Wave Based Enemy=======================================================
            anim.SetBool("canSeePlayer", canSeePlayer());
            //Line of Sight Enemy=======================================================
            if (playerInRange && enemyType != EnemyParams.EnemyType.Stationary && enemyDetection != EnemyParams.DetectionType.Wave && canSeePlayer()) // Check if the player is in range and the enemy is not a Stationary enemy
            {
                agent.stoppingDistance = stoppingDistanceOriginal; // Set the stopping distance of the NavMeshAgent to the original stopping distance
                if (enemyType == EnemyParams.EnemyType.Ranged)
                {
                    Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player on the horizontal plane
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate the enemy's body to face the player
                    agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                    if (!isAttacking && playerInRangedAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                    {
                        StartCoroutine(shoot()); // Start the shoot coroutine
                    }
                }


                if (enemyType == EnemyParams.EnemyType.Melee)
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
                if (!playerInRange && enemyType != EnemyParams.EnemyType.Stationary && enemyDetection != EnemyParams.DetectionType.Wave && !canSeePlayer() && roaming)
                {
                    StartCoroutine(roam()); // Call the roam function
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
        if(HP > 0)
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
                        anim.SetBool("playerInRange", true);
                    }
                    else
                    {
                        playerInRangedAttackRange = false;
                        anim.SetBool("playerInRange", false);
                    }
                }

                if (enemyType == EnemyParams.EnemyType.Melee)
                {
                    if (distance <= meleeRange) // Check if the player is in attack range
                    {
                        playerInMeleeAttackRange = true; // Set playerInAttackRange to true
                        anim.SetBool("playerInRange", true);
                    }
                    else
                    {
                        playerInMeleeAttackRange = false;
                        anim.SetBool("playerInRange", false);
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
            
            anim.SetTrigger("isDead"); // Set the trigger for the death animation
            StartCoroutine(Death()); // Start the death coroutine
        }
        else
        {
            agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the agent to the player's position
        }
    }

    IEnumerator Death() // Coroutine to handle the enemy's death
    {
        isAttacking = false; // Bool to check if the enemy is attacking
        playerInRange = false; // Bool to check if the player is in range of the enemy
        playerInMeleeAttackRange = false; // Bool to check if the player is in melee attack range of the enemy
        playerInRangedAttackRange = false; // Bool to check if the player is in ranged attack range of the enemy
        capsuleCollider.enabled = false; // Disable the capsule collider
        agent.isStopped = true; // Stop the agent from moving
        yield return new WaitForSeconds(2.5f); // Wait for the destroyTime from the EnemyParams scriptable object
        Destroy(gameObject); // Destroy the enemy
        GameManager.Instance.updateGameGoal(-1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one less enemy in the scene
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
        anim.SetBool("Attack", true); // Set the trigger for the attack animation
        anim.SetBool("isStopped", true); // Set the trigger for the idle animation
        yield return new WaitForSeconds(0.5f); // Wait for the attack animation to play
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

            // Optionally use a coroutine to smoothly translate the player to the new position
            Rigidbody playerRigidbody = GameManager.Instance.player.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                StartCoroutine(SmoothKnockback(playerRigidbody, knockbackDirection, knockbackDistance, 0.2f));
            }
        }

        yield return new WaitForSeconds(enemyParams.attackSpeed); // Wait while attack is ongoing

        agent.isStopped = false; // Re-enable movement
        isAttacking = false; // Set isAttacking to false
        anim.SetBool("Attack", false); // Set the trigger for the attack animation
        anim.SetBool("isStopped", false); // Set the trigger for the idle animation
    }

    IEnumerator SmoothKnockback(Rigidbody playerRigidbody, Vector3 direction, float distance, float duration) // Coroutine to smoothly knockback the player
    {
        float time = 0; // Initialize time to 0
        Vector3 startPosition = playerRigidbody.position; // Get the player's current position
        Vector3 targetPosition = startPosition + direction * distance; // Calculate the target position

        while (time < duration) // Loop while time is less than duration
        {
            playerRigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, time / duration)); // Smoothly move the player towards the target position
            time += Time.deltaTime; // Increment time by Time.deltaTime
            yield return null; // Wait for the next frame
        }
        playerRigidbody.MovePosition(targetPosition); // Ensure the player reaches the target position
    }



    bool canSeePlayer()
    {

        playerDir = GameManager.Instance.player.transform.position - headPos.position; // Get the direction to the player
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward); // Get the angle to the player
        

        if(enemyDetection == EnemyParams.DetectionType.Wave)
        {
            agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the agent to the player's position
            return true;
        }

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

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true; // Set destChosen to true
            agent.stoppingDistance = 0; // Set the stopping distance of the agent to 0

            yield return new WaitForSeconds(enemyParams.roamTimer); // Wait for the roam timer
            Vector3 ranPos = Random.insideUnitSphere * enemyParams.roamDist; // Get a random position within the roam distance
            ranPos += startingPos; // Add the starting position to the random position
            NavMeshHit hit; // Create a navmesh hit variable
            NavMesh.SamplePosition(ranPos, out hit, enemyParams.roamDist, 1); // Sample the position of the random position
            agent.SetDestination(hit.position); // Set the destination of the agent to the random position
            destChosen = false; // Set destChosen to false
        }


    }

}