using UnityEngine;
using System.Collections.Generic;

public class ToolBelt : MonoBehaviour
{
    public int[] potions; // Array to hold the counts of different potions

    public void AddPotion(int potionIndex, int count)
    {
        if (potionIndex >= 0 && potionIndex < potions.Length)
        {
            potions[potionIndex] = Mathf.Max(potions[potionIndex] + count, 0); // Ensure potion count doesn't go negative
        }
    }

    public int GetPotionCount(int potionIndex)
    {
        if (potionIndex >= 0 && potionIndex < potions.Length)
        {
            return potions[potionIndex];
        }
        return 0;
    }
}
