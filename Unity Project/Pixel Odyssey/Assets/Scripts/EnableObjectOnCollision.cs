using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnCollision : MonoBehaviour
{
    [SerializeField] GameObject m_Object;
    private void OnTriggerEnter(Collider other)
    {
        m_Object.SetActive(true);
    }
}
