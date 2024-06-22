//InventorySlot

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public int ID;
    public Item item;
    public int amount;

    public ItemDescriptionUI itemDescription;
    public DisplayInventory displayInventory;
    private Button button;

    public void Initialize(int _id, Item _item, int _amount, DisplayInventory displayInventory, ItemDescriptionUI itemDescription)     //constructor for the inventory slots
    {
        this.ID = _id;
        this.item = _item;
        this.amount = _amount;
        this.displayInventory = displayInventory;
        this.itemDescription = itemDescription;
    }
    //TODO: need to add listeners when the slots are created, either here or display inventory

    public void AddAmount(int value)
    {
        amount += value;
    }
    //TODO: Need to get the right click working

    public void OnClick()
    {
        if(item != null)
        {
            string description = item.Name + "\n" + itemDescription;
            itemDescription.UpdateDescription(description);
            displayInventory.SetSelectedSlot(this);
        }
    }
}