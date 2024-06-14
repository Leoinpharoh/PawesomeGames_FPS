using UnityEngine;
public class HealingPotion : MonoBehaviour
{
    public int potionIndex;
    public int potionCountIncrement = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToolBelt toolBelt = other.GetComponent<ToolBelt>();
            toolBelt.AddPotion(potionIndex, potionCountIncrement);

            // Update the UI
            int potionCount = toolBelt.GetPotionCount(potionIndex);
            GameManager.Instance.UpdatePotionSlotUI(potionIndex, potionCount);

            Debug.Log("Potion picked up. Index: " + potionIndex + ", Count: " + potionCount);

            Destroy(gameObject);
        }
    }
}
