using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();

        if(dmg != null)
        {
            Vector3 hitPosition = collision.contacts[0].point;
            if (collision.gameObject.CompareTag("Player")) // Check if the collided object is the player
            {
                dmg.takeDamage(damage, hitPosition);
            }
        }

        Destroy(gameObject);
    }

}
