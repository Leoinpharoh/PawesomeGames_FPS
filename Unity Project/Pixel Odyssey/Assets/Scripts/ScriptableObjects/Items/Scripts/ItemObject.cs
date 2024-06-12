//ItemObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType    //create the type of item you want
{
    Potion,
    Collectable,
    Weapon,
    AmmoPack,
    HealthPack,
    Default
}

public abstract class ItemObject : ScriptableObject  //base class for items
{
    public int ItemId;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)]   //text size for the description of the item
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int id;
    public Item(ItemObject item)
    {
        Name = item.name;
        id = item.ItemId;
    }
}