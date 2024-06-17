//Interact

using UnityEngine;

public class Interact : MonoBehaviour
{
    public float pickupRange;   //will be the distance a player can pickup from
    public DisplayInventory displayInventory;
    public PlayerManager playerManager;
    private Camera mainCamera;
    public PickUpMessage pickupMessage;

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
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickupRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();    //check for interactable
            GroundItem groundItem = hit.collider.GetComponent<GroundItem>();    //check if there is a grounditem hit
            if (groundItem != null)
            {
                HandlePickup(groundItem);
                hit.collider.gameObject.SetActive(false);   //deactivate the grounditem
            }
            if(interactable != null && groundItem == null)  //if it is interactable and not a ground item
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
    }
}
