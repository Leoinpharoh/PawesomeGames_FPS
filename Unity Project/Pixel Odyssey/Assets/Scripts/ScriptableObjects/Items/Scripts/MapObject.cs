using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Map Object", menuName = "Inventory System/Items/Map")]
public class MapObject : ItemObject
{
    //TODO: need to have something saying this is a map object than can be opened from the inventoryDisplay
    public void Awake()
    {
        type = ItemType.Map;
    }
}