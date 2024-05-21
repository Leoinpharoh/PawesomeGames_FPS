using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb; // Reference to the Rigidbody component

    public EnemyParams enemyParams; // Reference to the EnemyParams ScriptableObject

    void Start() // Start is called before the first frame update
    {
        if (enemyParams != null) 
        {
            rb.velocity = transform.forward * enemyParams.bulletSpeed; // Set the velocity of the bullet to the forward direction of the bullet multiplied by the speed
            Destroy(gameObject, enemyParams.destroyTime); // Destroy the bullet after the destroyTime
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (enemyParams != null) 
        {
            switch (enemyParams.damageType) // Switch statement to check the damage type of the bullet
            {
                case EnemyParams.DamageType.Regular: // Check if the damage type is Regular
                    HandleRegularDamage(collision);
                    break;
                case EnemyParams.DamageType.Poisoned: // Check if the damage type is Poisoned
                    HandlePoisonedDamage(collision);
                    break;
                case EnemyParams.DamageType.Burning: // Check if the damage type is Burning
                    HandleBurningDamage(collision);
                    break;
                case EnemyParams.DamageType.Freezing: // Check if the damage type is Freezing
                    HandleFreezingDamage(collision);
                    break;
                case EnemyParams.DamageType.Slowed: // Check if the damage type is Slowed
                    HandleSlowedDamage(collision);
                    break;
                case EnemyParams.DamageType.Confused: // Check if the damage type is Confused
                    HandleConfusedDamage(collision);
                    break;
            }

            Destroy(gameObject);
        }
    }

    private void HandleRegularDamage(Collision collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>(); // Get the IDamage component from the collided object
        if (dmg != null) // Check if the IDamage component is not null
        {
            Vector3 hitPosition = collision.contacts[0].point; // Get the hit position of the bullet
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                dmg.takeDamage(enemyParams.rangedDamage, hitPosition); // Call the takeDamage function from the IDamage component
            }
        }
    }

    private void HandlePoisonedDamage(Collision collision)
    {
        EDamage dmg = collision.gameObject.GetComponent<EDamage>(); // Get the EDamage component from the collided object
        IDamage Idmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null) // Check if the EDamage component is not null
        {
            Vector3 hitPosition = collision.contacts[0].point; // Get the hit position of the bullet
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                Idmg.takeDamage(enemyParams.rangedDamage, hitPosition); // Call the takeDamage function from the IDamage component
                dmg.poisonDamage(enemyParams.effectDamage, enemyParams.effectDuration); // Call the poisonDamage function from the EDamage component
            }
        }
    }

    private void HandleBurningDamage(Collision collision)
    {
        EDamage dmg = collision.gameObject.GetComponent<EDamage>(); // Get the EDamage component from the collided object
        IDamage Idmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null) // Check if the EDamage component is not null
        {
            Vector3 hitPosition = collision.contacts[0].point; // Get the hit position of the bullet
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                Idmg.takeDamage(enemyParams.rangedDamage, hitPosition); // Call the takeDamage function from the IDamage component
                dmg.burnDamage(enemyParams.effectDamage, enemyParams.effectDuration); // Call the burnDamage function from the EDamage component
            }
        }
    }

    private void HandleFreezingDamage(Collision collision) 
    {
        EDamage dmg = collision.gameObject.GetComponent<EDamage>(); // Get the EDamage component from the collided object
        IDamage Idmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null) // Check if the EDamage component is not null
        {
            Vector3 hitPosition = collision.contacts[0].point; // Get the hit position of the bullet
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                Idmg.takeDamage(enemyParams.rangedDamage, hitPosition); // Call the takeDamage function from the IDamage component
                dmg.freezeDamage(enemyParams.effectDamage, enemyParams.effectDuration); // Call the freezeDamage function from the EDamage component
            }
        }
    }

    private void HandleSlowedDamage(Collision collision) 
    {
        EDamage dmg = collision.gameObject.GetComponent<EDamage>(); // Get the EDamage component from the collided object
        IDamage Idmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null) // Check if the EDamage component is not null
        {
            Vector3 hitPosition = collision.contacts[0].point; // Get the hit position of the bullet
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                Idmg.takeDamage(enemyParams.rangedDamage, hitPosition); // Call the takeDamage function from the IDamage component
                dmg.slowDamage(enemyParams.effectDamage, enemyParams.effectDuration); // Call the slowDamage function from the EDamage component
            }
        }
    }
    private void HandleConfusedDamage(Collision collision)
    {
        EDamage dmg = collision.gameObject.GetComponent<EDamage>(); // Get the EDamage component from the collided object
        IDamage Idmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null) // Check if the EDamage component is not null
        {
            Vector3 hitPosition = collision.contacts[0].point; // Get the hit position of the bullet
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                Idmg.takeDamage(enemyParams.rangedDamage, hitPosition); // Call the takeDamage function from the IDamage component
                dmg.confuseDamage(enemyParams.effectDamage, enemyParams.effectDuration); // Call the slowDamage function from the EDamage component
            }
        }
    }
}
