using UnityEngine;

[CreateAssetMenu(fileName = "CurePotion", menuName = "Potions/CurePotion")]
public class CurePotion : Potion
{
    public override void Use(GameObject user)
    {
        PlayerManager playerManager = user.GetComponent<PlayerManager>();
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
}
