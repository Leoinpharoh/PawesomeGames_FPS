using UnityEngine;
public class HealingPotion : MonoBehaviour
{
    public string potionName = "Healing Potion";
    public int potionCount = 0;
    ToolBelt toolBelt;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!toolBelt.potions.ContainsKey("Healing Potion"))
            {
                toolBelt.potions.Add(potionName, potionCount++);
            }
            else if(toolBelt.potions.ContainsKey("Healing Potion"))
            {
                toolBelt.potions.TryGetValue(potionName, out potionCount);
                toolBelt.potions[potionName] = potionCount++;
            }
            //Destroying Object
            Destroy(gameObject);
        }
    }
}
