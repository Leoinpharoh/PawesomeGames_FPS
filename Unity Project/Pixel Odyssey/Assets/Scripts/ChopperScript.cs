using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopperScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sway();
    }
    
    IEnumerator sway()
    {
        transform.Rotate(-1f * Time.deltaTime, 0f, 0f);
        yield return new WaitForSeconds(1f);
    }
}
