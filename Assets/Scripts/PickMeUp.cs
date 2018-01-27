﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMeUp : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;
    public Collider2D plugCollider;
    public bool overPlug;
    public int plugNum;

    /*PLUG CHOICE ASPECT*/
    public GameObject phone;
    public GameObject mainCam;
    /********************/

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        //plugOrder = new int[mainCam.GetComponent<GameStuff>().PhoneText.Length];
    }
    private void Update()
    {
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint)+offset;
 transform.position = curPosition;

    }

    private void OnMouseUp()
    {
        if (overPlug == true)
        {
            transform.position = plugCollider.transform.position;
            if ((mainCam.GetComponent<GameStuff>().randPlug + 1) == plugCollider.GetComponent<Number>().Num)
            {
                Debug.Log("WUSSUP HOMIE");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plug"))
        {
            plugCollider = collision;
            overPlug = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plug"))
        {
            overPlug = false;
        }
    }
}
