//PuzzleHandler

using System.Collections;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void OnEnable()
    {
        PuzzleEventManager.StartListening("SlideFirstVineWall", OnSlideFirstVineWall);
    }

    private void OnDisable()
    {
        PuzzleEventManager.StopListening("SlideFirstVineWall", OnSlideFirstVineWall);
    }

    private void OnSlideFirstVineWall()
    {
        GameObject firstVineWall = GameObject.FindGameObjectWithTag("FirstVineWall");
        Vector3 targetPosition = new Vector3(5, 0, 0);
        float slideDuration = 2.0f;

        StartCoroutine(SlideObject(firstVineWall, targetPosition, slideDuration));
    }

    public IEnumerator SlideObject(GameObject obj, Vector3 targetPosition, float slideDuration, bool slideBack = false, float slideBackDelay = 1.0f)
    {
        initialPosition = obj.transform.position;   //get initial player position
        float slideTimer = 0f;

        while (slideTimer < slideDuration)
        {
            slideTimer += Time.deltaTime;
            float t = Mathf.Clamp(slideTimer / slideDuration, 0f, 1f);
            obj.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        if(slideBack)
        {
            yield return new WaitForSeconds(slideBackDelay);
            StartCoroutine(SlideObject(obj, initialPosition, slideDuration));
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
