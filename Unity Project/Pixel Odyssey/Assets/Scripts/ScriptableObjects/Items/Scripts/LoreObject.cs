using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Lore Object", menuName = "Inventory System/Items/LoreObject")]
public class LoreObject : ItemObject
{
    public int collectedAmount = 0;
    public int desiredAmount = 0;

    public void Awake()
    {
        type = ItemType.Lore;
    }
}