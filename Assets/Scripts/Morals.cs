using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morals : MonoBehaviour {

    public int level;
    public int policeMoral;
    public int correctPlug;
    public int F=0;
	public GameObject mainCam;

    // Use this for initialization
    void Start() {
        mainCam = GameObject.Find("Main Camera");
        if (level == 1)	//If this is level 1, Police Contact is reset
            PlayerPrefs.SetInt("Police Contact", 0);
		PlayerPrefs.SetInt ("Next Level", level + 1);		//Next level is the current level + 1
		policeMoral = PlayerPrefs.GetInt("Police Contact");	//Store Police Contact int in a local variable for easier use
    }
       
	void Update () {
        if (level == 2)
        {//If this is level 2, set up level 2's specific call
            correctPlug = 1;
            mainCam.GetComponent<GameStuff>().specificCall = 0;
            mainCam.GetComponent<GameStuff>().specificMessage = "Hello, I think I'm being followed... Oh shi- *gunshot*";
        }
        else if (level == 3 && policeMoral == 0)
        {//If this is level 3 and the police were not contacted in level 2
            if (F == 0)
            {//If this is the first specific call, set up the mafia call
                correctPlug = 5;
                mainCam.GetComponent<GameStuff>().specificCall = 3;
                mainCam.GetComponent<GameStuff>().specificMessage = "We know what you did, don't even deny it, if you value your life do not correctly connect detective Grant when he calls. Now connect me to Vito's";
                F++;
            }
            if (F == 1)	//If this is the second call, make the correct plug 1
                correctPlug = 1;
			//Set up the detective call
            mainCam.GetComponent<GameStuff>().specificCall = 5;
            mainCam.GetComponent<GameStuff>().specificMessage = "This is Dectective Grant. I must be connected immediately to the police station.";
        }
        else if (level == 3 && policeMoral == 1)
        {//Else if this is level 3 and the police WERE contacted in level 2, set up the mafia call
            mainCam.GetComponent<GameStuff>().specificCall = 3;
            mainCam.GetComponent<GameStuff>().specificMessage = "You think you're so smart huh, letting the fuzz know about our work. I hope you've said goodbye for yoour loved ones because we are coming for you";
        }
        else
        {
        }
    }

}

