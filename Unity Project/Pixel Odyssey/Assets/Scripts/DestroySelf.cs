using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] int Timer;
    void Start()
    {
        StartCoroutine(destoryer());
    }

    IEnumerator destoryer()
    {
        yield return new WaitForSeconds(Timer);
        Destroy(gameObject);
    }
}
