//RightClickMenu
//TODO: need to make pop up dynamically based on where the right click was

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenu : MonoBehaviour
{
    public DisplayInventory displayInventory;
    public GameObject rightClickMenuPanel;
    public Button dropButton;   //creating buttons for the drop down menu when we right click in the inventory
    public Button consumeButton;


    // Start is called before the first frame update
    void Start()
    {
        if (dropButton != null) dropButton.onClick.AddListener(DropItem);   //null checks to make sure buttons are assigned
        if (consumeButton != null) dropButton.onClick.AddListener(ConsumeItem);
        HideMenu();
    }

    public void ShowMenu(InventorySlot selectedSlot)
    {
        //if inventory is open
        //menu pops up at the location of the right click
        rightClickMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the rightClick menu
    /// </summary>
    public void HideMenu()
    {
        rightClickMenuPanel.SetActive(false);
    }

    public void DropItem()
    {

        HideMenu();
    }

    public void ConsumeItem()
    {

    }
}
