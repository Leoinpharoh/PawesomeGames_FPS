using UnityEngine;
public class OSPotion : MonoBehaviour
{
    public int potionIndex;
    public int potionCountIncrement = 1;
    PlayerManager playerManager;
    public SaveSystem saveSystem;
    bool PotionbeltUnlocked = false;
    private void OnTriggerEnter(Collider other)
    {
        PotionbeltUnlocked = saveSystem.playerData.PotionbeltUnlocked;
        if (other.CompareTag("Player"))
        {
            if (PotionbeltUnlocked)
            {
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
