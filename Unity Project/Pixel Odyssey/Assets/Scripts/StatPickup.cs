using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class StatPickup : MonoBehaviour
{
    [SerializeField] enum PickUpType {Health, Ammo, Cure}
    [SerializeField] PickUpType type;
    [SerializeField] int refilAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case PickUpType.Health:
                    other.gameObject.GetComponentInChildren<PlayerManager>().HP += refilAmount;
                    if(other.gameObject.GetComponent<PlayerManager>().HP > other.gameObject.GetComponent<PlayerManager>().HPOrignal) { other.gameObject.GetComponent<PlayerManager>().HP = other.gameObject.GetComponent<PlayerManager>().HPOrignal;  }
                    other.gameObject.GetComponent<PlayerManager>().updatePlayerUI();
                    break;
                case PickUpType.Ammo:
                    ShootingHandler[] shootingHandlers = other.gameObject.GetComponentsInChildren<ShootingHandler>(true);
                    foreach (var shootingHandler in shootingHandlers)
                    {
                        
                        shootingHandler.Ammo += refilAmount;
                        if((shootingHandler.Ammo + refilAmount) >= 99)
                        {
                            shootingHandler.Ammo = 99;
                        }
                    }
                    break;
                case PickUpType.Cure:
                    other.gameObject.GetComponent<PlayerManager>().poisoned = false;
                    GameManager.Instance.poisonHitScreen.SetActive(false);
                    other.gameObject.GetComponent<PlayerManager>().burning = false;
                    GameManager.Instance.burnHitScreen.SetActive(false);
                    other.gameObject.GetComponent<PlayerManager>().freezing = false;
                    GameManager.Instance.freezeHitScreen.SetActive(false);
                    other.gameObject.GetComponent<PlayerManager>().slowed = false;
                    GameManager.Instance.slowHitScreen.SetActive(false);
                    other.gameObject.GetComponent<PlayerManager>().confused = false;
                    GameManager.Instance.confuseHitScreen.SetActive(false);
                    other.gameObject.GetComponent<PlayerManager>().Normal = true;
                    other.gameObject.GetComponent<PlayerManager>().StopAllCoroutines();
                    GameManager.Instance.playerEffect("Normal");
                    other.gameObject.GetComponent<PlayerManager>().updatePlayerUI();
                    break;
}
            if(other.gameObject.GetComponentInChildren<ShootingHandler>() != null)
            {
                GameManager.Instance.playerAmmo(other.gameObject.GetComponentInChildren<ShootingHandler>().ammoType.ToString(), other.gameObject.GetComponentInChildren<ShootingHandler>().Ammo);
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
