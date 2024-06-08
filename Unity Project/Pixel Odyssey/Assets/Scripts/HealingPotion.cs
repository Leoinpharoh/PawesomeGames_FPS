using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/HealingPotion")]
public class HealingPotion : Potion
{
    public int healingAmount;

    public override void UsePotion(GameObject player)
    {
        var playerManager = player.GetComponent<PlayerManager>();
        playerManager.HP += healingAmount;
        if (playerManager.HP > playerManager.HPOrignal)
        {
            playerManager.HP = playerManager.HPOrignal;
        }
        playerManager.updatePlayerUI();
        Debug.Log("Used Healing Potion");
    }
}

