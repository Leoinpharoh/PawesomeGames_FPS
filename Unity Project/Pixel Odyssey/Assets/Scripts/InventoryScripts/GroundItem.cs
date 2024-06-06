//GroundItem

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundItem : MonoBehaviour
{
    public ItemObject scriptableObject;
    public GameObject originalPrefab;   //storing original prefab
    public Quaternion originalRotation; //storing original rotation
}
