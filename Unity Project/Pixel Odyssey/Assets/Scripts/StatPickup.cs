using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class StatPickup : MonoBehaviour
{
    [SerializeField] enum PickUpType {Health, Ammo}
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
                    break;
                case PickUpType.Ammo:
                    other.gameObject.GetComponentInChildren<ShootingHandler>().Ammo += refilAmount;
                    break;
            }
            GameManager.Instance.playerAmmo(other.gameObject.GetComponentInChildren<ShootingHandler>().ammoType.ToString(), other.gameObject.GetComponentInChildren<ShootingHandler>().Ammo);

            Destroy(this);
        }
    }
}
