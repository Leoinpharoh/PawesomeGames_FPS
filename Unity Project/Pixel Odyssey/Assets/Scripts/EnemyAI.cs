using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] SphereCollider triggerCollider;

    [SerializeField] EnemyParams enemyParams; // Reference to the EnemyParams ScriptableObject
    bool isAttacking = false;
    bool playerInRange = false;
    bool playerInMeleeAttackRange = false;
    bool playerInRangedAttackRange = false;
    float meleeRange;
    float rangedRange;
    private Transform playerTransform;

    EnemyParams.EnemyType enemyType; // references the enemy type from the EnemyParams scriptable object
    EnemyParams.DetectionType enemyDetection; // references the enemy detection from the EnemyParams scriptable object

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.updateGameGoal(1);

        enemyType = enemyParams.enemyType;
        enemyDetection = enemyParams.detectionType;
        triggerCollider.radius = enemyParams.lineOfSightRange;
        triggerCollider.isTrigger = true;
        agent.acceleration = enemyParams.Acceleration;
        agent.speed = enemyParams.movementSpeed;
        meleeRange = enemyParams.meleeRange;
        rangedRange = enemyParams.rangedRange;
        playerTransform = GameManager.Instance.player.transform; // Get the player's transform from the GameManager
        if (enemyType == EnemyParams.EnemyType.Ranged)
        {
            agent.stoppingDistance = enemyParams.rangedRange - 3;
            if (agent.stoppingDistance < 1)
            {
                agent.stoppingDistance = 1;
            }
        }
        else
        {
            agent.stoppingDistance = enemyParams.rangedRange;
        }




    }

    // Update is called once per frame
    void Update()
    {
//Wave Based Enemy=======================================================
        if (enemyDetection == EnemyParams.DetectionType.Wave)
        {
            agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
            if (!isAttacking && enemyType == EnemyParams.EnemyType.Ranged && playerInRange) // Check if the enemy is a Ranged enemy and is not shooting
            {
                StartCoroutine(shoot()); // Start the shoot coroutine
            }
        }
//Wave Based Enemy=======================================================

//Line of Sight Enemy=======================================================
        if (playerInRange && enemyType != EnemyParams.EnemyType.Stationary && enemyDetection != EnemyParams.DetectionType.Wave) // Check if the player is in range and the enemy is not a Stationary enemy
        {
            if (enemyType == EnemyParams.EnemyType.Ranged)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player
                agent.SetDestination(GameManager.Instance.player.transform.position); // Set the destination of the NavMeshAgent to the player's position
                if (!isAttacking) // Check if the enemy is a Ranged enemy and is not shooting
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
            if (!isAttacking) // Check if the enemy is a Ranged enemy and is not shooting
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

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
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
        enemyParams.HP -= amount; // Subtract the amount from the enemy's health
        StartCoroutine(Damage(hitPosition)); // Start the flash coroutine
        if (enemyParams.HP <= 0) // Check if the enemy's health is less than or equal to 0
        {
            GameManager.Instance.updateGameGoal(-1); // Call the updateGameGoal function from the gameManager script. tells game manager that there is one less enemy in the scene
            Destroy(gameObject); // Destroy the enemy
        }
    }

    IEnumerator Damage(Vector3 hitPosition)
    {
        GameObject bloodEffect = Instantiate(enemyParams.bloodSplash, hitPosition, Quaternion.identity); // Instantiate at the hit position
        bloodEffect.transform.SetParent(transform); // Optionally set the parent to the enemy's transform
        yield return new WaitForSeconds(1); // Wait for 1 second (adjust based on your effect's needs)
        Destroy(bloodEffect); // Optionally destroy the effect after it finishes playing

    }
}