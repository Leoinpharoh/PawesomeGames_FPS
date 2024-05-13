using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Health Object", menuName = "Inventory System/Items/Health")]
public class HealthObject : ItemObject
{
    public int restoreHealthValue;  //value which the health item will restore
    public void Awake()
    {
        type = ItemType.Health; //auto creates health item
    }
}