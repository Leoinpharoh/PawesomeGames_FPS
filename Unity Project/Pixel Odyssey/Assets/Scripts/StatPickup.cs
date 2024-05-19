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
                    other.gameObject.GetComponentInChildren<ShootingHandler>().Ammo += refilAmount;
                    break;
                    case PickUpType.Cure:
                    other.gameObject.GetComponent<PlayerManager>().poisoned = false;
                    other.gameObject.GetComponent<PlayerManager>().burning = false;
                    other.gameObject.GetComponent<PlayerManager>().freezing = false;
                    other.gameObject.GetComponent<PlayerManager>().slowed = false;
                    other.gameObject.GetComponent<PlayerManager>().confused = false;

                    break;
            }
            GameManager.Instance.playerAmmo(other.gameObject.GetComponentInChildren<ShootingHandler>().ammoType.ToString(), other.gameObject.GetComponentInChildren<ShootingHandler>().Ammo);

            Destroy(this.gameObject);
        }
    }
}
