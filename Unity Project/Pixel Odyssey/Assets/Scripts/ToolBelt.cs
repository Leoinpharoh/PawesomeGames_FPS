using UnityEngine;
using System.Collections.Generic;

public class ToolBelt : MonoBehaviour
{
    public List<Potion> potions = new List<Potion>();
    private int selectedPotionIndex = 0;

    public void AddPotion(Potion potion)
    {
        potions.Add(potion);
        UpdateSelectedPotionUI();
    }

    public void UsePotion()
    {
        if (potions.Count > 0)
        {
            potions[selectedPotionIndex].Use(gameObject);
            potions.RemoveAt(selectedPotionIndex);
            selectedPotionIndex = Mathf.Clamp(selectedPotionIndex, 0, potions.Count - 1);
            UpdateSelectedPotionUI();
        }
    }

    public void ScrollPotions(int direction)
    {
        if (potions.Count == 0) return;

        selectedPotionIndex += direction;
        if (selectedPotionIndex >= potions.Count)
            selectedPotionIndex = 0;
        else if (selectedPotionIndex < 0)
            selectedPotionIndex = potions.Count - 1;

        UpdateSelectedPotionUI();
    }

    public Potion GetSelectedPotion()
    {
        if (potions.Count > 0)
        {
            return potions[selectedPotionIndex];
        }
        return null;
    }

    public int GetSelectedPotionCount()
    {
        if (potions.Count > 0)
        {
            return potions.Count;
        }
        return 0;
    }

    private void UpdateSelectedPotionUI()
    {
        GameManager.Instance.UpdateSelectedPotion();
    }
}
