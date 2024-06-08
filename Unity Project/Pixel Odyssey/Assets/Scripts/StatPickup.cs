using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPickup : MonoBehaviour
{
    [SerializeField] private enum PickUpType { Health, Ammo, Cure }
    [SerializeField] private PickUpType type;
    [SerializeField] private int refilAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                var toolBelt = playerManager.GetComponent<ToolBelt>();

                // If player has a ToolBelt, add the potion to it
                if (toolBelt != null)
                {
                    switch (type)
                    {
                        case PickUpType.Health:
                            HealingPotion healingPotion = ScriptableObject.CreateInstance<HealingPotion>();
                            healingPotion.potionName = "Healing Potion";
                            healingPotion.healingAmount = refilAmount;
                            toolBelt.AddPotion(healingPotion);
                            break;

                        case PickUpType.Cure:
                            CurePotion curePotion = ScriptableObject.CreateInstance<CurePotion>();
                            curePotion.potionName = "Cure Potion";
                            toolBelt.AddPotion(curePotion);
                            break;

                        case PickUpType.Ammo:
                            RefillAmmo(playerManager);
                            break;
                    }
                }
                else
                {
                    // If player does not have a ToolBelt, apply the effect directly
                    switch (type)
                    {
                        case PickUpType.Health:
                            ApplyHealth(playerManager);
                            break;

                        case PickUpType.Cure:
                            ApplyCure(playerManager);
                            break;

                        case PickUpType.Ammo:
                            RefillAmmo(playerManager);
                            break;
                    }
                }
            }
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    private void ApplyHealth(PlayerManager playerManager)
    {
        playerManager.HP += refilAmount;
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
        playerManager.StopAllCoroutines();
        GameManager.Instance.playerEffect("Normal");
        playerManager.updatePlayerUI();
    }

    private void RefillAmmo(PlayerManager playerManager)
    {
        ShootingHandler[] shootingHandlers = playerManager.gameObject.GetComponentsInChildren<ShootingHandler>(true);
        foreach (var shootingHandler in shootingHandlers)
        {
            shootingHandler.weaponStats.Ammo += refilAmount;
            if (shootingHandler.weaponStats.Ammo > 99)
            {
                shootingHandler.weaponStats.Ammo = 99;
            }
        }
        if (playerManager.gameObject.GetComponentInChildren<ShootingHandler>() != null)
        {
            GameManager.Instance.playerAmmo(
                playerManager.gameObject.GetComponentInChildren<ShootingHandler>().weaponStats.ammoType.ToString(),
                playerManager.gameObject.GetComponentInChildren<ShootingHandler>().weaponStats.Ammo);
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
