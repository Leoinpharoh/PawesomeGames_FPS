using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurePotion : MonoBehaviour
{
        public int potionIndex; // Index for the type of potion in ToolBelt
        public int potionCountIncrement = 1; // Amount to add to ToolBelt when picked up

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToolBelt toolBelt = other.GetComponent<ToolBelt>();
            
                // Add potion to the ToolBelt
                toolBelt.AddPotion(potionIndex, potionCountIncrement);

                // Update the UI
                GameManager.Instance.UpdatePotionSlotUI(potionIndex, toolBelt.GetPotionCount(potionIndex));

                // Destroy the potion GameObject
                Destroy(gameObject);

        }
    }
}

