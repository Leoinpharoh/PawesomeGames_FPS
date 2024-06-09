using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportDest;
    public GameObject telePlayer;

    private void OnTriggerEnter(Collider other)
    {
        telePlayer.transform.position = teleportDest.transform.position;
    }
}
