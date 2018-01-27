using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Vector3 mousePos;
    Camera mainCam;
    Vector3 screenPoint;
    Vector3 scanPos;
    Vector3 offset;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
    }
    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;
        mousePos = mainCam.ScreenToWorldPoint(mousePos);
    }
}