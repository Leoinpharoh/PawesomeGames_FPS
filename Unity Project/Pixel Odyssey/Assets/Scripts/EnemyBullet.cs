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
    [SerializeField] float duration;

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
                    dmg.poisonDamage(damage, duration);
                }
            }
        }
        if (damageType == DamageType.Burning)
        {
            EDamage dmg = collision.gameObject.GetComponent<EDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.burnDamage(damage, duration);
                }
            }
        }
        if (damageType == DamageType.Freezing)
        {
            EDamage dmg = collision.gameObject.GetComponent<EDamage>();

            if (dmg != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
                {
                    dmg.freezeDamage(damage, duration);
                }
            }
        }

        Destroy(gameObject);
    }

    

}
