using System.Collections;
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
            mainCam.GetComponent<GameStuff>().phoneUp();//ANIMATE PHONE PICKUP HERE
        }
        else if(pickedUp)
        {
            //ANIMATE PHONE DROP HERE
            mainCam.GetComponent<GameStuff>().StopAllCoroutines();//ANIMATE PHONE PICKUP HERE
            dialogue.text = ("");
        }
        pickedUp = !pickedUp;
    }
}
