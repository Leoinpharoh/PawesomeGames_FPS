using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Collectable Object", menuName = "Inventory System/Items/Collectable")]
public class CollectableObject : ItemObject
{
    public int collectNumber;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Collectable;
    }
}
