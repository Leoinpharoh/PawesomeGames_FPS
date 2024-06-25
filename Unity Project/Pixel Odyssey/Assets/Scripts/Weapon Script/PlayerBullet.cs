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
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        dmg = collision.gameObject.GetComponent<IDamage>();
        sDMG = collision.gameObject.GetComponent<SDamage>();

        if (!collision.gameObject.CompareTag("Player"))
        {
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
}

public class DamageSphere : MonoBehaviour
{
    public int damage;
    public float destructionTime;
    public float radius = 4.5f;

    private void Start()
    {
        StartCoroutine(ApplyDamage());
        // Destroy the sphere collider object after the specified time
        Destroy(gameObject, destructionTime);
    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private IEnumerator ApplyDamage()
    {
        // Wait for a fixed update to ensure all collisions are processed
        yield return new WaitForFixedUpdate();

        // Get all colliders within the specified radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Player"))
            {
                IDamage dmg = hitCollider.GetComponent<IDamage>();
                SDamage sDMG = hitCollider.GetComponent<SDamage>();

                if (dmg != null)
                {
                    Vector3 hitPosition = hitCollider.ClosestPoint(transform.position);
                    dmg.takeDamage(damage, hitPosition);
                }

                if (sDMG != null)
                {
                    sDMG.ObjectDamage(damage);
                }
            }
        }
    }
}
