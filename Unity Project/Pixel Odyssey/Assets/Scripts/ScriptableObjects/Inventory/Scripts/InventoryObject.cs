//InventoryObject

using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath; //used to save inventory to a path
    public ItemDatabaseObject database;
    public Inventory Container;

    private void OnEnable()
    {
        LoadDatabase();
    }
    private void OnValidate()
    {
        LoadDatabase();
    }

    private void LoadDatabase()
    {
        //SimpleConsole.Instance.Log("Loading database...");
        if (database == null)
        {
            database = Resources.Load<ItemDatabaseObject>("Database");
            /*if (database == null)
            {
                SimpleConsole.Instance.Log("Failed to load database from Resources!");
            }
            else
            {
                SimpleConsole.Instance.Log($"Database loaded. Item count: {database.Items.Length}");
                foreach (var item in database.Items)
                {
                    SimpleConsole.Instance.Log($"Item in database: ID={item.ItemId}, Name={item.name}");
                }
            }*/
        }
        /*else
        {
            SimpleConsole.Instance.Log($"Database already loaded. Item count: {database.Items.Length}");
        }*/
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
        GameObject newSlotObj = new GameObject("InventorySlot_" + _item.id);
        InventorySlot newSlot = newSlotObj.AddComponent<InventorySlot>();   //giving the new object an inventory slot component
        newSlot.Initialize(_item.id, _item, _amount, FindObjectOfType<DisplayInventory>(), FindObjectOfType<ItemDescriptionUI>());

        Container.Items.Add(newSlot);
    }

    public void RemoveItem(int id)
    {
        Container.Items.RemoveAll(slot => slot.ID == id);
    }

    //TODO: Need to implement save/load functions
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