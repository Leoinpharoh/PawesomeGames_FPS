using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPickup : MonoBehaviour
{
    [SerializeField] private enum PickUpType { Health, Ammo, Cure }
    [SerializeField] private PickUpType type;
    [SerializeField] private int refillAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {

                switch (type)
                {
                    case PickUpType.Health:
                        HandleHealthPickup(playerManager);
                        break;

                    case PickUpType.Cure:
                        HandleCurePickup(playerManager);
                        break;

                    case PickUpType.Ammo:
                        RefillAmmo(playerManager);
                        break;
                }
            }
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    private void HandleHealthPickup(PlayerManager playerManager)
    {
        if (playerManager.HP < playerManager.HPOrignal)
        {
            ApplyHealth(playerManager);
        }
    }

    private void HandleCurePickup(PlayerManager playerManager)
    {
        if (playerManager.poisoned || playerManager.burning || playerManager.freezing ||
            playerManager.slowed || playerManager.confused)
        {
            ApplyCure(playerManager);
        }
    }

    private void ApplyHealth(PlayerManager playerManager)
    {
        playerManager.HP += refillAmount;
        if (playerManager.HP > playerManager.HPOrignal)
        {
            playerManager.HP = playerManager.HPOrignal;
        }
        playerManager.updatePlayerUI();
    }

    private void ApplyCure(PlayerManager playerManager)
    {
        playerManager.poisoned = false;
        playerManager.burning = false;
        playerManager.freezing = false;
        playerManager.slowed = false;
        playerManager.confused = false;
        playerManager.Normal = true;
        playerManager.StopCoroutine(playerManager.poisonCoroutine);
        playerManager.StopCoroutine(playerManager.burnCoroutine);
        playerManager.StopCoroutine(playerManager.slowCoroutine);
        playerManager.StopCoroutine(playerManager.freezeCoroutine);
        playerManager.StopCoroutine(playerManager.confuseCoroutine);
        GameManager.Instance.playerEffect("Normal");
        playerManager.updatePlayerUI();
    }

    private void RefillAmmo(PlayerManager playerManager)
    {
        ShootingHandler[] shootingHandlers = playerManager.gameObject.GetComponentsInChildren<ShootingHandler>();
        foreach (var shootingHandler in shootingHandlers)
        {
            int maxAmmoLimit = 99; // Set your max ammo limit here
            if (shootingHandler.weaponStats.ammoType == WeaponStats.AmmoType.Light) { shootingHandler.weaponStats.Ammo += 10; }
            else if (shootingHandler.weaponStats.ammoType == WeaponStats.AmmoType.Medium) { shootingHandler.weaponStats.Ammo += 6; }
            else if (shootingHandler.weaponStats.ammoType == WeaponStats.AmmoType.Heavy) { shootingHandler.weaponStats.Ammo += 1; }
            if (shootingHandler.weaponStats.Ammo > maxAmmoLimit)
            {
                shootingHandler.weaponStats.Ammo = maxAmmoLimit;
            }
        }

        if (playerManager.gameObject.GetComponentInChildren<ShootingHandler>() != null)
        {
            var shootingHandler = playerManager.gameObject.GetComponentInChildren<ShootingHandler>();
            GameManager.Instance.playerAmmo(
                shootingHandler.weaponStats.ammoType.ToString(),
                shootingHandler.weaponStats.Ammo);
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
