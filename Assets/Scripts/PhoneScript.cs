using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScript : MonoBehaviour {

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
            //ANIMATE PHONE PICKUP HERE
            pickedUp = true;
        }
        else if(pickedUp)
        {
            //ANIMATE PHONE DROP HERE
            mainCam.GetComponent<GameStuff>().finishedCall = true;
            pickedUp = false;
        }

    }
}
