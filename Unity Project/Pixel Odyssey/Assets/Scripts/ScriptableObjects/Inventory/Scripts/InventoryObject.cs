//InventoryObject

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath; //used to save inventory to a path
    public ItemDatabaseObject database;
    public Inventory Container;

    private void Awake()
    {
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Scripts/ScriptableObjects/Items/Database.asset", typeof(ItemDatabaseObject));
    }

    public void AddItem(Item _item, int _amount)
    {

        for (int i = 0; i < Container.Items.Count; i++)   //looping through the container/slot
        {
            if (Container.Items[i].item.id == _item.id) //if we have the item already
            {
                Container.Items[i].AddAmount(_amount);    //add item to the list
                return;
            }
        }

        Container.Items.Add(new InventorySlot(_item.id, _item, _amount));   //else add a new container to that slot
    }

    public void RemoveItem(int id)
    {
        Container.Items.RemoveAll(slot => slot.ID == id);
    }

    //[ContextMenu("Save")]
    public void Save()  //not implemented
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    //[ContextMenu("Load")]
    public void Load()  //not implemented
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            InventorySlot slot = Container.Items[i];
            ItemObject itemObject = database.GetItem[slot.ID];
            slot.item = new Item(itemObject); // Create a new Item instance with the retrieved ItemObject
        }
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}