//ItemDatabaseObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver //fires before Unity Serializes object
{
    public ItemObject[] Items;  //array full of the items
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
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
        //every time Unity serializes the object it auto populates dictionary
    }


    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}