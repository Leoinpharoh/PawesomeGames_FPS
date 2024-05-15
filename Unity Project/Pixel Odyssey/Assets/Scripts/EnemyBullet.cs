using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    [HideInInspector] public enum DamageType { Regular, Poisoned, Burning, Freezing, Slowed, Confused }
    public DamageType damageType;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(damageType == DamageType.Regular)
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.takeDamage(damage, hitPosition);
                }
            }
        }
        if (damageType == DamageType.Poisoned)
        {
            EDamage dmg = collision.gameObject.GetComponent<EDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.poisonDamage("Poisoned");
                }
            }
        }
        if (damageType == DamageType.Burning)
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.takeDamage(damage, hitPosition);
                }
            }
        }
        if (damageType == DamageType.Freezing)
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.takeDamage(damage, hitPosition);
                }
            }
        }
        if (damageType == DamageType.Slowed)
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.takeDamage(damage, hitPosition);
                }
            }
        }
        if (damageType == DamageType.Confused)
        {
            IDamage dmg = collision.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.takeDamage(damage, hitPosition);
                }
            }
        }

        Destroy(gameObject);
    }

    

}
