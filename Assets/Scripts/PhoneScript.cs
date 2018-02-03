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

	//Function that fires on mouseclick
    private void OnMouseDown()
    {
        if (!pickedUp)
		{//If the phone hasn't been picked up yet
			/*Stop playing the current sound and instead play the phonePickup sound*/
			mainCam.GetComponent<GameStuff>().source.Stop();
			mainCam.GetComponent<GameStuff>().source.PlayOneShot(mainCam.GetComponent<GameStuff>().phonePickup);

            if (mainCam.GetComponent<GameStuff>().callActive)
			{//If there is supposed to be a call
				/*Pick up the phone, animate the phone being picked up, reset the timer to 0, and reset the phone's rotation/rotation variable.*/
                mainCam.GetComponent<GameStuff>().phoneUp();
                GetComponent<Animator>().SetBool("phoneAnswered", true);
                mainCam.GetComponent<GameStuff>().timer = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                mainCam.GetComponent<GameStuff>().k = 0;
            }
			else//If there isn't supposed to be a call
           		dialogue.text = ("...There is no call right now...");	//Display this
        }
        else if(pickedUp)
        {//If the phone HAS been picked up
			/*Animate the phone being put down, play the phonePutdown sound, and stop the dialogue from updating and remove it.*/
            GetComponent<Animator>().SetBool("phoneAnswered", false);
			mainCam.GetComponent<GameStuff>().source.PlayOneShot(mainCam.GetComponent<GameStuff>().phonePutdown);
            mainCam.GetComponent<GameStuff>().StopAllCoroutines();
            dialogue.text = ("");
        }
        pickedUp = !pickedUp;	//pickedUp is equal to the opposite of what it just was
    }
}
