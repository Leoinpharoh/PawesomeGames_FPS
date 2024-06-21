using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    [SerializeField] float glitchTime;
    Material material;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Random.value < glitchTime)
        {
            if (material.HasProperty("GlitchStrength"))
            {
                material.SetFloat("GlitchStrength", Random.Range(0f, 1f));
            }
        }else
        {
            if (material.HasProperty("GlitchStrength"))
            {
                material.SetFloat("GlitchStrength", 0f);
            }
        }
    }
}
