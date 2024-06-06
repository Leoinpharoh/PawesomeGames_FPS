//InventoryManager

using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public DisplayInventory displayInventory;
    public GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Returning bool if there is an inventory or not
    /// </summary>
    /// <returns></returns>
    public bool HasInventory()
    {
        return displayInventory != null;
    }

    /// <summary>
    /// returns a DisplayInventory
    /// </summary>
    /// <returns></returns>
    public DisplayInventory GetInventory()
    {
        return displayInventory;
    }

    public void ToggleInventory()
    {
        Debug.Log("ToggleInventory method called");

        if (inventoryUI == null)    // if UI is null return debug warning and return
        {
            Debug.LogWarning("Cannot toggle inventory: inventoryUI GameObject is not assigned");
            return;
        }

        if (!inventoryUI.activeSelf)   //if inventory is not active
        {
            Debug.Log("Setting the inventory to active");
            inventoryUI.SetActive(true);   //set to active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            displayInventory = inventoryUI.GetComponent<DisplayInventory>();    //ensuring displayInventory is correct

            if (displayInventory != null)
            {
                displayInventory.CreateDisplay();
            }
            else
                Debug.LogWarning("DisplayInventory componnent not found on inventoryUi.");
        }
        else
        {
            Debug.Log("Setting the inventory to inactive");
            inventoryUI.SetActive(false);  //if inventory is active deactivate and lock mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}