using UnityEngine;

[CreateAssetMenu(fileName = "HealingPotion", menuName = "Potions/HealingPotion")]
public class HealingPotion : Potion
{
    public int healingAmount;

    public override void Use(GameObject user)
    {
        PlayerManager playerManager = user.GetComponent<PlayerManager>();
        playerManager.HP += healingAmount;
        if (playerManager.HP > playerManager.HPOrignal)
        {
            playerManager.HP = playerManager.HPOrignal;
        }
        playerManager.updatePlayerUI();
    }
}
