using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New AmmoPack Object", menuName = "Inventory System/Items/AmmoPack")]
public class AmmoPack : ItemObject
{
    public int amount;
    public string ammoType;
    public void Awake()
    {
        type = ItemType.Default;
    }
}