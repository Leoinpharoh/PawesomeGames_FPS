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
    private SDamage sDMG;
    private Vector3 hitPosition;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        dmg = collision.gameObject.GetComponent<IDamage>();
        sDMG = collision.gameObject.GetComponent<SDamage>();

        Instantiate(Explode, gameObject.transform);

        if (dmg != null)
        {
            hitPosition = collision.contacts[0].point;
            dmg.takeDamage(damage, hitPosition);
            Destroy(gameObject);
        }
        if (sDMG != null)
        {
            hitPosition = collision.contacts[0].point;
            sDMG.ObjectDamage(damage);
            Destroy(gameObject);
        }
        
        Destroy(gameObject);
    }
}