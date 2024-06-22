//Interact

using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float pickupRange;   //will be the distance a player can pickup from
    public DisplayInventory displayInventory;
    public PlayerManager playerManager;
    public PickUpMessage pickupMessage;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   //get the reference to the camera
    }

    public void InteractWithObject()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = mainCamera.transform.position;
        Vector3 raycastDirection = mainCamera.transform.forward;
        Debug.DrawRay(raycastOrigin, raycastDirection, Color.red, 2f);
        
        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, pickupRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();    //check for interactable
            GroundItem groundItem = hit.collider.GetComponent<GroundItem>();    //check if there is a grounditem hit
            if (groundItem != null) //if it is a groundItem we call HandlePickup
            {
                HandlePickup(groundItem);
                hit.collider.gameObject.SetActive(false);
            }
            else if(interactable != null)  //if it is interactable we trigger interact
            {
                interactable.Interact(playerManager, displayInventory, pickupMessage);
            }
        }
    }

    public void HandlePickup(GroundItem groundItem)
    {
        ItemObject itemObject = groundItem.scriptableObject;

        playerManager.itemObjectToGroundItemMap[itemObject] = groundItem;
        Item _item = new Item(itemObject);
        displayInventory.PickUpItem(_item, 1);
        pickupMessage.ShowPickUpPanel(itemObject.pickUpMessage);

        //checking for keys
        if(itemObject is VineKeyObject)
        {
            displayInventory.vineKeyCount++;
        }
        if(itemObject is MazeKeyObject)
        {
            displayInventory.mazeKeyCount++;
        }
        if(itemObject is GlyphObject)
        {
            displayInventory.glyphCount++;
        }
    }
}
