using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPermaBoost : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] enum PickUpType { HealthPlus, OverShieldPlus }
    [SerializeField] PickUpType type;
    [SerializeField] int boostAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case PickUpType.HealthPlus:
                    other.gameObject.GetComponent<PlayerManager>().HP += boostAmount;
                    other.gameObject.GetComponent<PlayerManager>().HPOrignal += boostAmount;
                    break;
                case PickUpType.OverShieldPlus:
                    other.gameObject.GetComponent<PlayerManager>().OS += boostAmount;
                    other.gameObject.GetComponent<PlayerManager>().OSOrignal += boostAmount;
                    break;
            }
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
