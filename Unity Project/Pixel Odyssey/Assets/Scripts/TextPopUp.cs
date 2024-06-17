//TextPopUp

using TMPro;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    public Transform playerTransform;
    public TextMeshPro textMeshPro;
    public float displayDistance;
    public AudioSource triggerSound;
    private bool hasPlayed = false;
    private bool isDisplaying = false;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //calculating distance to player
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <= displayDistance)    //if less than the display distance
        {
            if (!isDisplaying)
            {
                textMeshPro.gameObject.SetActive(true);
                if(!hasPlayed)  //if the sound has played dont play again
                    triggerSound.Play();
                isDisplaying = true;
            }
        }
        else
        {
            if (isDisplaying)
            {
                textMeshPro.gameObject.SetActive(false);
                isDisplaying = false;
                hasPlayed = true;   //once turned off change hasPlayed to true so it doesn't play the sound again
            }
        }
    }
}
