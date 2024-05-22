using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] float flipAngle = 90f;

    private Camera _camera;
    private SpriteRenderer _Renderer;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = _camera.transform.position;
        Vector3 lookDir = (cameraPos - transform.position).normalized;
        
        float angle = Vector3.Angle(transform.forward, lookDir);
        if (angle < flipAngle) { _Renderer.sprite = frontSprite; }
        else { _Renderer.sprite = backSprite;}
        //transform.LookAt(cameraPos, Vector3.up);
    }
}
