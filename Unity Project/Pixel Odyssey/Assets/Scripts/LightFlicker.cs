using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] Light light; // The light to flicker
    [SerializeField] float flashRate; // The rate at which the light will flicker

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < flashRate) // Randomly turn the light on or off
        {
            light.enabled = !light.enabled; // Toggle the light
        }
    }
}
