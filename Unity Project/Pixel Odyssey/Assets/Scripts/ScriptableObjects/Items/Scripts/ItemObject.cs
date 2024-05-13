using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,
    Collectable,
    Weapon,
    Default
}

public abstract class ItemObject : ScriptableObject  //base class for items
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(15,20)]   //text size for the description of the item
    public string description;
}
