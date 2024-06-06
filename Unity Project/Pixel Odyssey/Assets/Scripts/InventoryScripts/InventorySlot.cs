//InventorySlot

using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)     //constructor for the inventory slots
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter called");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit called");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick called");
    }
}