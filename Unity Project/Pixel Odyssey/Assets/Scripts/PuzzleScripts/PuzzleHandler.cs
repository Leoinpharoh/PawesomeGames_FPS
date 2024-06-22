//PuzzleHandler

using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private int slideCount = 0;
    private bool isMazeChestOpen = false;
    [SerializeField] private DisplayInventory displayInventory;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// Slides the first door covered with vines
    /// </summary>
    public void OnSlideVineWall()
    {
        //if displayInventory.keycount
        if (slideCount <= 0 && displayInventory.vineKeyCount >= 3)  //if it hasn't slid and there is 3 vinekeys
        {
            GameObject firstVineWall = GameObject.Find("FirstVineWall");
            firstVineWall.GetComponent<MeshCollider>().enabled = false;
            Vector3 slideOffset = new Vector3(3, 0, 0);
            Vector3 targetPosition = firstVineWall.transform.localPosition + firstVineWall.transform.TransformDirection(slideOffset); ;
            float slideDuration = 2.0f;
            StartCoroutine(SlideObject(firstVineWall, targetPosition, slideDuration));
            slideCount++;
        }
        else
            return;
    }

    /// <summary>
    /// Rotates the maze chest lid
    /// </summary>
    public void OnRotateMazeChest()
    {
        if (!isMazeChestOpen && displayInventory.mazeKeyCount >= 5)
        {
            GameObject mazeChestLid = GameObject.Find("mazeChestLid");
            if (mazeChestLid != null)
            {
                Quaternion initialRotation = mazeChestLid.transform.localRotation;
                Quaternion targetRotation = Quaternion.Euler(-90, 0, 0);
                float rotateDuration = 2.0f;
                StartCoroutine(RotateObject(mazeChestLid, initialRotation, targetRotation, rotateDuration));
                isMazeChestOpen = true;
            }
        }
    }

    public bool ChestTryOpen(GameObject chestLid)
    {
        if (chestLid != null)
        {
            ChestOpen(chestLid);
            return true;
        }
        return false;
    }

    public void ChestOpen(GameObject chestLid)
    {
        Quaternion initialRotation = chestLid.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(-90, 0, 0);
        float rotateDuration = 2.0f;
        StartCoroutine(RotateObject(chestLid, initialRotation, targetRotation, rotateDuration));
    }

    public IEnumerator SlideObject(GameObject obj, Vector3 targetPosition, float slideDuration, bool slideBack = false, float slideBackDelay = 1.0f)
    {
        initialPosition = obj.transform.localPosition;   //get initial wall local position
        float slideTimer = 0f;

        while (slideTimer < slideDuration)
        {
            slideTimer += Time.deltaTime;
            float t = Mathf.Clamp(slideTimer / slideDuration, 0f, 1f);
            obj.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        if(slideBack)
        {
            yield return new WaitForSeconds(slideBackDelay);
            StartCoroutine(SlideObject(obj, initialPosition, slideDuration));
        }
    }

    public IEnumerator RotateObject(GameObject obj, Quaternion initialRotation, Quaternion targetRotation, float rotateDuration, bool rotateBack = false, float rotateBackDelay = 1.0f)
    {
        float rotateTimer = 0f;

        while (rotateTimer < rotateDuration)
        {
            rotateTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotateTimer / rotateDuration);

            t = Mathf.SmoothStep(0f, 1f, t);

            obj.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }
    }

    /// <summary>
    /// Resets the object to original position and rotation
    /// </summary>
    /// <param name="obj"></param>
    public void ResetObject(GameObject obj)
    {
        obj.transform.position = initialPosition;
        obj.transform.rotation = initialRotation;
    }
}
