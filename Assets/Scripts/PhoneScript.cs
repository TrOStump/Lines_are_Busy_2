﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneScript : MonoBehaviour {

    public Text dialogue;
    GameObject phone;
    GameObject mainCam;

    public bool pickedUp = false;
	// Use this for initialization
	void Start () {
        mainCam = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
	}
    private void OnMouseDown()
    {

        if (!pickedUp)
        {
            if (mainCam.GetComponent<GameStuff>().callActive) {
                mainCam.GetComponent<GameStuff>().phoneUp();//ANIMATE PHONE PICKUP HERE
                mainCam.GetComponent<GameStuff>().timer = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                mainCam.GetComponent<GameStuff>().k = 0;
            }
        else
            dialogue.text = ("...There is no call right now...");
        }
        else if(pickedUp)
        {
            //ANIMATE PHONE DROP HERE
            mainCam.GetComponent<GameStuff>().StopAllCoroutines();
            dialogue.text = ("");
        }
        pickedUp = !pickedUp;
    }
}
