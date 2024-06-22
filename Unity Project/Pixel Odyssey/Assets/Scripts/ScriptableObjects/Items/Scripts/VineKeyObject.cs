using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Vine Key Object", menuName = "Inventory System/Items/VineKey")]
public class VineKeyObject : ItemObject
{
    [SerializeField] private int currentCount = 0; //what we have now
    public int desiredAmount = 0;   //what you need

    public void Awake()
    {
        type = ItemType.VineKey;
    }
}