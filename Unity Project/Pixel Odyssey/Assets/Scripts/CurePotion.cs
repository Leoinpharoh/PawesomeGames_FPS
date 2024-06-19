using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CurePotion : MonoBehaviour
{
    public int potionIndex; // Index for the type of potion in ToolBelt
    public int potionCountIncrement = 1; // Amount to add to ToolBelt when picked up
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

                // Add potion to the ToolBelt
                toolBelt.AddPotion(potionIndex, potionCountIncrement);

                // Update the UI
                int potionCount = toolBelt.GetPotionCount(potionIndex);
                GameManager.Instance.UpdatePotionSlotUI(potionIndex, toolBelt.GetPotionCount(potionIndex));

                // Destroy the potion GameObject
                Destroy(gameObject);
            }
        }
    }
}

