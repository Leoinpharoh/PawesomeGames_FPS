//InventoryManager

using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public DisplayInventory displayInventory;
    public GameObject inventoryUI;


    void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI.SetActive(false);
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

        if (!inventoryUI.activeSelf)   //if inventory is not active
        {
            inventoryUI.SetActive(true);   //set to active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            displayInventory = inventoryUI.GetComponent<DisplayInventory>();    //ensuring displayInventory is correct

            if (displayInventory != null)
            {
                displayInventory.CreateDisplay();
            }
        }
        else
        {
            inventoryUI.SetActive(false);  //if inventory is active deactivate and lock mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}