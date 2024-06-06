//Interact

using UnityEngine;

public class Interact : MonoBehaviour
{
    public float pickupRange;   //will be the distance a player can pickup from
    public DisplayInventory displayInventory;
    public PlayerManager playerManager;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   //get the reference to the camera
        if (mainCamera == null)
        {
            Debug.Log("Main Camera not found");
        }
    }

    public void PickupItem()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = mainCamera.transform.position;
        Vector3 raycastDirection = mainCamera.transform.forward;
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * pickupRange, Color.red, pickupRange);   //raycas

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickupRange))
        {
            Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * pickupRange, Color.green, pickupRange); //raycast turns green when it hits an object

            GroundItem groundItem = hit.collider.GetComponent<GroundItem>();    //check if there is a grounditem hit
            if (groundItem != null)
            {
                Debug.Log("Rest of the PickupItem method is working");
                ItemObject itemObject = groundItem.scriptableObject;

                playerManager.itemObjectToGroundItemMap[itemObject] = groundItem;
                Item _item = new Item(itemObject);
                displayInventory.PickUpItem(_item, 1);

                hit.collider.gameObject.SetActive(false);   //deactivate the grounditem
            }
        }
    }
}
