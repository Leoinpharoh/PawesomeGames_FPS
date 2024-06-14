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
    [SerializeField] AudioSource audioSource; // Reference to the AudioSource component
    public Rigidbody playerRigidbody; // Reference to the player's Rigidbody component
    bool isAttacking = false; // Bool to check if the enemy is attacking
    bool playerInRange = false; // Bool to check if the player is in range of the enemy
    bool playerInMeleeAttackRange = false; // Bool to check if the player is in melee attack range of the enemy
    bool playerInRangedAttackRange = false; // Bool to check if the player is in ranged attack range of the enemy
    bool roaming = false; // Bool to check if the enemy is roaming
    bool destChosen;
    bool isDead;
    int HP; // Enemy Health
    int stoppingDistanceOriginal; // Original stopping distance of the NavMeshAgent
    float meleeRange; // Enemy Attack Range
    float rangedRange; // Enemy Attack Range
    float attackLeadingtime; // Leading time for the enemy's attack
    Vector3 playerDir; // Vector3 to store the direction to the player
    Vector3 startingPos; // Vector3 to store the starting position of the enemy
    float angleToPlayer; // Float to store the angle to the player
    private Transform playerTransform; // Reference to the player's transform
    bool isReloading;
    int shootLoop;

    EnemyParams.EnemyType enemyType; // references the enemy type from the EnemyParams scriptable object
    EnemyParams.DetectionType enemyDetection; // references the enemy detection from the EnemyParams scriptable object
    PlayerManager playerManager; // Reference to the PlayerManager script


    // Start is called before the first frame update
    void Start()
    {
        Startup();
    }

    // Update is called once per frame
    void Update()
    {
        AICheck();
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
                        anim.SetBool("meleeRange", true);
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
                        anim.SetBool("meleeRange", false);
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
        if (isAttacking) yield break; // Prevent concurrent shooting routines
        if (isDead) // Check if the enemy is dead
        {
            yield break; // Prevent attacking if dead
        }
        isAttacking = true;
        PlayAttackSound();
        anim.SetBool("Attack", true);
        anim.SetBool("isStopped", true);

        float projectileSpeed = enemyParams.bulletSpeed; // Get the speed of the projectile
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); // Get the distance to the player

        // Get the move direction from the PlayerManager
        Vector3 playerMoveDirection = playerManager.GetCurrentMoveDirection() * playerManager.moveSpeed;

        // Predict the player's future position
        Vector3 predictedPosition = playerTransform.position + (playerMoveDirection * attackLeadingtime);
        Vector3 shootDirection = (predictedPosition - shootPos.position).normalized;
        Quaternion shootRotation = Quaternion.LookRotation(shootDirection);

        // Instantiate the bullet and orient it towards the predicted position
        Instantiate(enemyParams.bullet, shootPos.position, shootRotation);

        yield return new WaitForSeconds(enemyParams.attackSpeed); // Wait based on attack speed

        anim.SetBool("Attack", false);
        anim.SetBool("isStopped", false);
        isAttacking = false;
    }
   
    IEnumerator Reload()
    {
        isReloading = true;
        anim.SetBool("isReloading", true);
        PlayReloadSound();
        yield return new WaitForSeconds(enemyParams.enemyReloadTime);
        anim.SetBool("isReloading", false);
        isReloading = false;
    }


    public void takeDamage(int amount, Vector3 hitPosition) // Method to take damage
    {
        HP -= amount; // Subtract the amount from the enemy's health
        StartCoroutine(Damage(hitPosition)); // Start the flash coroutine
        if (HP <= 0) // Check if the enemy's health is less than or equal to 0
        {
            anim.SetTrigger("isDead"); // Set the trigger for the death animation
            isDead = true; // Set isDead to true
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
        agent.speed = 0;
        agent.angularSpeed = 0;
        PlayDeathSound();
        yield return new WaitForSeconds(2.5f); // Wait for the destroyTime from the EnemyParams scriptable object
        Destroy(gameObject); // Destroy the enemy
        //GameManager.Instance.updateGameGoal(-1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one less enemy in the scene
        Vector3 dropPosition = new Vector3 (transform.position.x, transform.position.y + 2, transform.position.z);
        if (enemyParams.lootPinata)
        {
            int chance = Random.Range(0, 100);
            if(chance <= enemyParams.lootChance)
            {
                Instantiate(enemyParams.loot[Random.Range(0, enemyParams.loot.Length)], dropPosition, Quaternion.identity); // Instantiate the loot at the enemy's position
            }
        }
    }

    IEnumerator Damage(Vector3 hitPosition)
    {
        GameObject bloodEffect = Instantiate(enemyParams.bloodSplash, hitPosition, Quaternion.identity); // Instantiate at the hit position
        bloodEffect.transform.SetParent(transform); // Optionally set the parent to the enemy's transform
        PlayDamageSound();
        yield return new WaitForSeconds(1); // Wait for 1 second (adjust based on your effect's needs)
        Destroy(bloodEffect); // Optionally destroy the effect after it finishes playing
    }

    IEnumerator meleeAttack() // Coroutine to handle the enemy's attack
    {
        if (isAttacking) // Check if the enemy is already attacking
        {
            yield break; // Prevent multiple simultaneous attacks
        }

        if (isDead) // Check if the enemy is dead
        {
            yield break; // Prevent attacking if dead
        }
        isAttacking = true; // Set isAttacking to true
        PlayAttackSound();
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
            /*Transform playerTransform = GameManager.Instance.player.transform;
            Vector3 knockbackDirection = (playerTransform.position - transform.position).normalized;
            float knockbackDistance = 2.0f; // Customize the distance as needed

            // Optionally use a coroutine to smoothly translate the player to the new position
            Rigidbody playerRigidbody = GameManager.Instance.player.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                StartCoroutine(SmoothKnockback(playerRigidbody, knockbackDirection, knockbackDistance, 0.2f));
            }*/
        }

        yield return new WaitForSeconds(enemyParams.attackSpeed); // Wait while attack is ongoing

        agent.isStopped = false; // Re-enable movement
        isAttacking = false; // Set isAttacking to false
        anim.SetBool("Attack", false); // Set the trigger for the attack animation
        anim.SetBool("meleeRange", false); // Set the trigger for the melee animation
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
        if (isDead) // Check if the enemy is dead
        {
            return false; // Return false
        }
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
        if (!destChosen)
        {
            destChosen = true;
            startingPos = transform.position;
            yield return new WaitForSeconds(enemyParams.roamTimer); // Wait for the roam timer
            Vector3 ranPos = Random.insideUnitSphere * enemyParams.roamDist; // Get a random position within the roam distance
            ranPos += startingPos; // Add the starting position to the random position
            NavMeshHit hit; // Create a navmesh hit variable
            NavMesh.SamplePosition(ranPos, out hit, enemyParams.roamDist, 1); // Sample the position of the random position
            agent.SetDestination(hit.position); // Set the destination of the agent to the random position
            destChosen = false;
        }
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && enemyParams.attackSound.Length > 0)
        {
            AudioClip clip = enemyParams.attackSound[Random.Range(0, enemyParams.attackSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayReloadSound()
    {
        if (audioSource != null && enemyParams.reloadSound.Length > 0)
        {
            AudioClip clip = enemyParams.reloadSound[Random.Range(0, enemyParams.reloadSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && enemyParams.deathSound.Length > 0)
        {
            AudioClip clip = enemyParams.deathSound[Random.Range(0, enemyParams.deathSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && enemyParams.damagedSound.Length > 0)
        {
            AudioClip clip = enemyParams.damagedSound[Random.Range(0, enemyParams.damagedSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayIdleSound()
    {
        if (audioSource != null && enemyParams.idleSound.Length > 0)
        {
            AudioClip clip = enemyParams.idleSound[Random.Range(0, enemyParams.idleSound.Length)];
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PlayWalkingSound()
    {
        if (audioSource != null && enemyParams.walkingSound.Length > 0)
        {
            AudioClip clip = enemyParams.walkingSound[Random.Range(0, enemyParams.walkingSound.Length)];
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void Startup()
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
        attackLeadingtime = enemyParams.rangedAttackLead; // Set the attackLeadingtime to the rangedAttackLead from the EnemyParams scriptable object
        playerTransform = GameManager.Instance.player.transform; // Get the player's transform from the GameManager
        agent.stoppingDistance = enemyParams.rangedRange; // Set the stopping distance of the NavMeshAgent to the rangedRange from the EnemyParams scriptable object
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (playerRigidbody == null && playerTransform != null)
        {
            playerRigidbody = playerTransform.GetComponent<Rigidbody>();
        }
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player"); // Adjust tag as necessary
        if (playerGameObject != null)
        {
            playerManager = playerGameObject.GetComponent<PlayerManager>();
        }
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
            agent.stoppingDistance = enemyParams.meleeRange - 1; // Set the stopping distance of the NavMeshAgent to the meleeRange from the EnemyParams scriptable object
            if (agent.stoppingDistance < 1) // Check if the stopping distance is less than 1
            {
                agent.stoppingDistance = 1; // Set the stopping distance to 1
            }
        }
        stoppingDistanceOriginal = (int)agent.stoppingDistance; // Set the stoppingDistanceOriginal to the stopping distance of the NavMeshAgent
    }


    private void AICheck()
    {
        if (isDead)
        {
            return;
        }
        if(enemyType == EnemyParams.EnemyType.Ranged && shootLoop >= enemyParams.enemyClipSize)
        {
            StartCoroutine(Reload());
            shootLoop = 0;
        }
        if (HP > 0 && !isReloading)
        {
            float animSpeed = agent.velocity.normalized.magnitude; // Get the speed of the agent
            float targetAnimSpeed = agent.velocity.magnitude / agent.speed;
            animSpeed = Mathf.MoveTowards(animSpeed, targetAnimSpeed, agent.acceleration * Time.deltaTime);
            anim.SetFloat("Speed", animSpeed);

            if (animSpeed > 0 && !audioSource.isPlaying)
            {
                PlayWalkingSound();
            }
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
                    shootLoop++;
                    StartCoroutine(shoot()); // Start the shoot coroutine
                }

                if (!isAttacking && (enemyType == EnemyParams.EnemyType.Melee || enemyType == EnemyParams.EnemyType.Combination) && playerInMeleeAttackRange)
                {
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
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                    agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                    if (!isAttacking && playerInRangedAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                    {
                        shootLoop++;
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
                        shootLoop++;
                        StartCoroutine(shoot()); // Start the shoot coroutine
                    }
                }

            }
            if (enemyType != EnemyParams.EnemyType.Stationary && enemyDetection != EnemyParams.DetectionType.Wave && !canSeePlayer() && roaming)
            {

                StartCoroutine(roam()); // Call the roam function
            }


            //Line of Sight Enemy=======================================================

            //Stationary Enemy=======================================================
            if (playerInRange && enemyType == EnemyParams.EnemyType.Stationary)
            {
                Vector3 predictedPlayerPosition = playerTransform.position + playerRigidbody.velocity * attackLeadingtime;
                Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player
                if (!isAttacking && playerInRangedAttackRange) // Check if the enemy is a Ranged enemy and is not shooting
                {
                    shootLoop++;
                    StartCoroutine(shoot()); // Start the shoot coroutine
                }
            }
            //Stationary Enemy=======================================================
        }
    }

}