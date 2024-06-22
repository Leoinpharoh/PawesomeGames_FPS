using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Glyph Object", menuName = "Inventory System/Items/Glyph")]
public class GlyphObject : ItemObject
{
    private int collectNumber;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Glyph;
    }
}
