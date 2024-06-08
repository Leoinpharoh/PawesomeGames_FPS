using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBelt : MonoBehaviour
{
    [SerializeField] private List<Potion> potions;
    [SerializeField] private int maxPotions = 10;
    private int selectedPotionIndex = -1; // -1 means no potion is selected

    private void Start()
    {
        // Initialize potions list with empty slots
        potions = new List<Potion>(new Potion[maxPotions]);
    }

    public void AddPotion(Potion newPotion)
    {
        for (int i = 0; i < potions.Count; i++)
        {
            if (potions[i] == null)
            {
                potions[i] = newPotion;
                Debug.Log("Potion added: " + newPotion.name);
                return;
            }
        }
        Debug.Log("Tool belt is full!");
    }

    public void UsePotion(int index)
    {
        if (index < 0 || index >= potions.Count || potions[index] == null)
        {
            Debug.Log("No potion in this slot!");
            return;
        }

        potions[index].UsePotion(gameObject);
        potions[index] = null;  // Remove potion after use
    }

    public void SelectNextPotion()
    {
        int startingIndex = selectedPotionIndex;
        do
        {
            selectedPotionIndex = (selectedPotionIndex + 1) % potions.Count;
        } while (potions[selectedPotionIndex] == null && selectedPotionIndex != startingIndex);

        Debug.Log($"Selected potion: {GetSelectedPotionName()}");
    }

    public void SelectPreviousPotion()
    {
        int startingIndex = selectedPotionIndex;
        do
        {
            selectedPotionIndex = (selectedPotionIndex - 1 + potions.Count) % potions.Count;
        } while (potions[selectedPotionIndex] == null && selectedPotionIndex != startingIndex);

        Debug.Log($"Selected potion: {GetSelectedPotionName()}");
    }

    public void UseSelectedPotion()
    {
        if (selectedPotionIndex >= 0 && selectedPotionIndex < potions.Count && potions[selectedPotionIndex] != null)
        {
            UsePotion(selectedPotionIndex);
            Debug.Log("Potion used: " + selectedPotionIndex);
        }
        else
        {
            Debug.Log("No potion selected or potion slot is empty!");
        }
    }

    public string GetSelectedPotionName()
    {
        if (selectedPotionIndex >= 0 && selectedPotionIndex < potions.Count && potions[selectedPotionIndex] != null)
        {
            return potions[selectedPotionIndex].potionName;
        }
        return "None";
    }
}