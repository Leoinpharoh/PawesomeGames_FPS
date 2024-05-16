using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();

        if (dmg != null)
        {
            Vector3 hitPosition = collision.contacts[0].point;
            if (!collision.gameObject.CompareTag("Player"))
            {
                dmg.takeDamage(damage, hitPosition);
            }
        }
    }
}