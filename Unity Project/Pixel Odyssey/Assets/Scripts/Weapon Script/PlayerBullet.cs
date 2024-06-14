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
    public bool useSphereColliderForDamage = false;
    public float sphereColliderRadius = 5f;
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

        if (collision.contacts.Length > 0)
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Instantiate(Explode, collisionPoint, Quaternion.identity);

            if (useSphereColliderForDamage)
            {
                // Create a sphere collider at the collision point
                GameObject damageSphere = new GameObject("DamageSphere");
                damageSphere.transform.position = collisionPoint;
                SphereCollider sphereCollider = damageSphere.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = true;
                sphereCollider.radius = sphereColliderRadius;

                // Add DamageSphere component to handle the damage logic
                DamageSphere damageSphereComponent = damageSphere.AddComponent<DamageSphere>();
                damageSphereComponent.damage = damage;
                damageSphereComponent.destructionTime = destroyTime; // Destroy the sphere collider after the bullet destroy time
            }
        }

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

public class DamageSphere : MonoBehaviour
{
    public int damage;
    public float destructionTime;

    private void Start()
    {
        // Destroy the sphere collider object after the specified time
        Destroy(gameObject, destructionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        SDamage sDMG = other.GetComponent<SDamage>();

        if (dmg != null)
        {
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            dmg.takeDamage(damage, hitPosition);
        }

        if (sDMG != null)
        {
            sDMG.ObjectDamage(damage);
        }
    }
}
