using UnityEngine;
public class HealingPotion : MonoBehaviour
{
    public int potionIndex;
    public int potionCountIncrement = 1;
    PlayerManager playerManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerManager.potionbeltUnlocked) {
                ToolBelt toolBelt = other.GetComponent<ToolBelt>();
                toolBelt.AddPotion(potionIndex, potionCountIncrement);

                // Update the UI
                int potionCount = toolBelt.GetPotionCount(potionIndex);
                GameManager.Instance.UpdatePotionSlotUI(potionIndex, potionCount);


                Destroy(gameObject);
            }
               
        }
    }
}
