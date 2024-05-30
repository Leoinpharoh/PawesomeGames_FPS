using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    public int speed;
    public int destroyTime;


    public GameObject Explode;

    private IDamage dmg;
    private Vector3 hitPosition;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        dmg = collision.gameObject.GetComponent<IDamage>();

        if (dmg != null)
        {
            hitPosition = collision.contacts[0].point;
            dmg.takeDamage(damage, hitPosition);
            Destroy(gameObject);
        }
        Instantiate(Explode, collision.transform);
        Destroy(gameObject);
    }
}