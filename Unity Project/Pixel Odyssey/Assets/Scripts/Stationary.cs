using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stationary : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;

    [SerializeField] int HP;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] private GameObject bloodSplash; // Creates a reference to the blood splash

    bool isShooting;
    bool playerInRange;

    private Transform playerTransform;

    void Start()
    {
        GameManager.Instance.updateGameGoal(1);
        playerTransform = GameManager.Instance.player.transform; // Get the player's transform from the GameManager
    }

    void Update()
    {
        if (playerInRange)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized; // Get the direction to the player
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Create a rotation to face the player
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate to face the player

            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
}
