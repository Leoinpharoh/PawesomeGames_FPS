//ItemDatabaseObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
[ExecuteInEditMode]
public class ItemDatabaseObject : ScriptableObject //fires before Unity Serializes object
{
    public ItemObject[] Items;  //array full of the items
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        UpdateDatabase();
    }

    public void OnValidate()
    {
        UpdateDatabase();
    }

    public void UpdateDatabase()
    {
        GetItem = new Dictionary<int, ItemObject>();
        if (Items != null)     //if the array is not null
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)   //if that item is not null
                {
                    Items[i].ItemId = i;    //item ID gets set during serialization
                    if (!GetItem.ContainsKey(Items[i].ItemId))  //if it does not contain
                        GetItem.Add(Items[i].ItemId, Items[i]);
                }
            }
        }
    }
}