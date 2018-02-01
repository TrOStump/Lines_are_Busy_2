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
			mainCam.GetComponent<GameStuff>().source.Stop();
			mainCam.GetComponent<GameStuff>().source.PlayOneShot(mainCam.GetComponent<GameStuff>().phonePickup);

            if (mainCam.GetComponent<GameStuff>().callActive)
			{
                mainCam.GetComponent<GameStuff>().phoneUp();
                GetComponent<Animator>().SetBool("phoneAnswered", true);
                mainCam.GetComponent<GameStuff>().timer = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                mainCam.GetComponent<GameStuff>().k = 0;
            }
			else
           		dialogue.text = ("...There is no call right now...");
        }
        else if(pickedUp)
        {
            GetComponent<Animator>().SetBool("phoneAnswered", false);
            mainCam.GetComponent<GameStuff>().StopAllCoroutines();
            mainCam.GetComponent<GameStuff>().source.PlayOneShot(mainCam.GetComponent<GameStuff>().phonePutdown);
            dialogue.text = ("");
        }
        pickedUp = !pickedUp;
    }
}
