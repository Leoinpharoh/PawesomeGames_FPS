//DisplayInventory

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class DisplayInventory : MonoBehaviour
{
    public PlayerManager player;
    public GameObject inventoryPrefab;
    public RightClickMenu rightClickMenu;
    public InventoryObject inventory;
    public GameObject inventoryGrid;
    public GameObject inventoryUI;  //reference to the UI panel
    public InventorySlot selectedSlot;  //for dropping/using purposes
    public ItemDescriptionUI itemDescription;   //reference to the items description attached to the scriptable

    public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// iterating over itemsDisplayed and giving them objects and sprites to appear in the inventory screen
    /// </summary>
    public void CreateDisplay()
    {

        List<InventorySlot> slotsToDisplay
            = inventory.Container.Items.Where(slot => !itemsDisplayed.ContainsKey(slot)).ToList();  //create a list of slots not already in itemsDisplayed
        foreach (InventorySlot slot in slotsToDisplay)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryGrid.transform); //creates a inventoryprefab to be used in the inventory display

            ItemObject itemObject = inventory.database.GetItem[slot.item.id];   //assigning our itemObject to corresponding item in the database
            Sprite itemSprite = itemObject.uiDisplay;   //getting the sprite from that item

            obj.GetComponent<UnityEngine.UI.Image>().sprite = itemSprite;   //assinging sprite to the component of the inventoryprefab
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");    //assigning the amount text in the slot

            itemsDisplayed.Add(slot, obj); //adding that slot to the display along with the object tied to it
        }
    }

    public void UpdateDisplay()
    {
        Dictionary<InventorySlot, GameObject> updatedItemsDisplayed = new Dictionary<InventorySlot, GameObject>();

        List<InventorySlot> slotsToRemove
            = inventory.Container.Items.Where(slot => !itemsDisplayed.ContainsKey(slot)).ToList();  //if it contains a key that is not in itemsDisplayed
        foreach (InventorySlot slot in slotsToRemove)
            RemoveSlot(slot);   //calling RemoveSlot to remove the slot not in itemsDisplayed

        foreach (InventorySlot slot in inventory.Container.Items)
        {
            if (itemsDisplayed.ContainsKey(slot))   //if we have one in the itemsDisplayed
            {
                GameObject obj = itemsDisplayed[slot];  //create an obj for that item
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

                ItemObject itemObject = inventory.database.GetItem[slot.item.id];
                Sprite itemSprite = itemObject.uiDisplay;

                obj.GetComponent<UnityEngine.UI.Image>().sprite = itemSprite;

                updatedItemsDisplayed.Add(slot, obj);  //adding that slot to the updated display
            }
            else //if there is not one in the inventory already, make a new object
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryGrid.transform);

                ItemObject itemObject = inventory.database.GetItem[slot.item.id];
                Sprite itemSprite = itemObject.uiDisplay;

                obj.GetComponent<UnityEngine.UI.Image>().sprite = itemSprite;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

                slot.SetDependancies(this, itemDescription);    //setting the inventoryDisplay, and the description
                updatedItemsDisplayed.Add(slot, obj);
            }
        }

        itemsDisplayed.Clear(); //clearing original itemsDisplayed
        foreach (var entry in updatedItemsDisplayed)    //adding back all the items to items displayed
        {
            itemsDisplayed.Add(entry.Key, entry.Value);
        }
        updatedItemsDisplayed.Clear();  //clearing the updated list for next time we need to update
    }
    public void PickUpItem(Item itemToPickup, int amount)
    {

        inventory.AddItem(itemToPickup, amount);    //add the item to out InventoryObject

        UpdateDisplay();
    }

    /// <summary>
    /// Sets the current slot
    /// </summary>
    /// <param name="slot"></param>
    public void SetSelectedSlot(InventorySlot slot)
    {
        selectedSlot = slot;
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = null;
    }

    public void DropItem()
    {
        if (inventory.Container.Items.Count > 0)    //checking to see if there is any items in the container
        {
            if (selectedSlot != null)
            {
                //finding where to drop the object
                ItemObject itemObject = inventory.database.GetItem[selectedSlot.ID];    //assigns itemObject as the itemObject that is found at selectedSlot.ID

                if (player.itemObjectToGroundItemMap.TryGetValue(itemObject, out GroundItem groundGameObject))  //if there is a value at index itemObject, assign that to groundGameObject
                {
                    Transform playerTransform = groundGameObject.transform; //setting a transform local to the player
                    Vector3 dropPosition = playerTransform.position + playerTransform.forward * 2f; //calculating the drop position in front of the player

                    groundGameObject.originalPrefab.SetActive(true);   //re-activating the original item
                    groundGameObject.transform.position = dropPosition; //setting its new position
                }

                if (selectedSlot.amount > 1)
                {
                    selectedSlot.amount--;  //if greater than one, remove 1 from the total
                }
                else
                {   //removing items
                    InventorySlot slotToRemove
                        = itemsDisplayed.Keys.FirstOrDefault(slot => slot.ID == selectedSlot.ID);  //finding the slot with matching id to dropped item and assigning it to slot to remove
                    if (slotToRemove != null)
                    {
                        RemoveSlot(slotToRemove);   //remove from itemsDisplayed and set slot to null
                        inventory.RemoveItem(slotToRemove.item.id); //remove from inventory container
                    }
                }
                UpdateDisplay();

            }
        }
    }

    /// <summary>
    /// Removes the slot from itemsDisplayed and destroys the slot object
    /// </summary>
    /// <param name="slotToRemove"></param>
    public void RemoveSlot(InventorySlot slotToRemove)
    {
        if (itemsDisplayed.ContainsKey(slotToRemove))    //removing the slot from items displayed that is not present in the container
        {
            GameObject objToDestroy = itemsDisplayed[slotToRemove];

            if (objToDestroy != null)
            {
                Destroy(objToDestroy);
                itemsDisplayed[slotToRemove] = null;
            }
            itemsDisplayed.Remove(slotToRemove);
        }

    }
}