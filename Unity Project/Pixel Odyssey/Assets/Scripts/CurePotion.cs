using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/CurePotion")]
public class CurePotion : Potion
{
    public override void UsePotion(GameObject player)
    {
        var playerManager = player.GetComponent<PlayerManager>();
        playerManager.poisoned = false;
        playerManager.burning = false;
        playerManager.freezing = false;
        playerManager.slowed = false;
        playerManager.confused = false;
        playerManager.Normal = true;
        playerManager.StopAllCoroutines();
        GameManager.Instance.playerEffect("Normal");
        playerManager.updatePlayerUI();
        Debug.Log("Used Cure Potion");
    }
}
