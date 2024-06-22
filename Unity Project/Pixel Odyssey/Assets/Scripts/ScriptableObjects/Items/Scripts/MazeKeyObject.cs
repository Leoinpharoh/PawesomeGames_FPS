using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Maze Key Object", menuName = "Inventory System/Items/MazeKey")]
public class MazeKeyObject : ItemObject
{
    [SerializeField] private int currentCount = 0; //what we have now
    public int desiredAmount = 0;   //what you need

    public void Awake()
    {
        type = ItemType.MazeKey;
    }
}