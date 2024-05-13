using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingWB : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;

    [SerializeField] int HP;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;

    // Start is called before the first frame update
    void wake()
    {
        GameManager.Instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.Instance.player.transform.position);
        if (playerInRange)
        {
            if(!isShooting )
            {
                StartCoroutine(shoot());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
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
        isShooting=false;
    }
    public void takeDamage(int damageamount, Vector3 hitpositon)
    {
        HP -= damageamount;

        if(HP <= 0)
        {
            GameManager.Instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
}
