using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStuff : MonoBehaviour {
	/*SCORE MECHANICS*/
    public int overallScore = 0;
    public float happyTimer = 0;
    public int customersServed = 0;
    public int customersSatisfied = 0;

	/*SPECIAL DIALOGUE MECHANICS*/
    public int specificCall;
    public bool specificBool=false;
    public string specificMessage;
    public int specificPlug;

	/*VARIOUS INTEGERS*/
    public int i;
    private int j;
    public int k = 0;
    public int S;
	public int linguistics;

	/*GAME OBJECTS*/
    public GameObject button;
    public GameObject phone;
    public GameObject cord;

	/*TEXT MECHANICS*/
    public Text dialogue;
    public string[] OpeningText;
    public string[] PhoneText;
    public string[] PhoneNumbers;
    public string[] Locations;
	private int currentlyDisplayingText = 0;

	/*PLUG MECHANICS*/
    public int currentCall;
    public int randPlug;
    public bool callActive;
    //public bool ye = true;

	/*PHONE RING MECHANICS*/
	public AudioClip ringing;
	public AudioClip phonePickup;
	public AudioClip phonePutdown;
	public AudioSource source;
    private int randShake;
    private Collider2D phoneCollider;

    /*TIME MECHANICS*/
    public float shiftLength;      //3 minutes of gameplay
    public float phoneTimer;       //New phonecall every 15 seconds (12 calls in first shift)
    public float timer = -11f;
    public int callNumber = 0;
    /****************/

    private void Start()    //Initialize stuff here
    {
        source = gameObject.GetComponent<AudioSource>();
		StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));	//Display opening text.
    }

    public void Update()	//Bulk of the coding
    {
        if (j == (OpeningText.Length)-1)	//If the button has been pressed one time less than how long the opening text is
            removeButton();


        /*TIME MECHANICS*/
        if (shiftLength > 0)				//If the shift isn't over
            shiftLength -= Time.deltaTime;	//Countdown
		
		else if (shiftLength < 0 && shiftLength >= -10) {		//If the shift is over AND THE SHIFT STARTED (-11 is the sentinel)
            shiftLength = 0f;									//Shift is set at 0
            
			/*Set the day's scores and load the scoreboard*/
			PlayerPrefs.SetInt("Served", customersServed);
            PlayerPrefs.SetInt("Satisfied", customersSatisfied);
            PlayerPrefs.SetInt("Overall", overallScore);
            SceneManager.LoadScene("Scoreboard");
        }

        if(happyTimer > 0)	//If the phone was picked up and the customer is waiting to be connected
        {
            happyTimer -= Time.deltaTime;
        }
        if (timer > -10)	//If the timer for the phonecalls has been started
        {
            timer += Time.deltaTime;	//Count upward
            if (timer >= phoneTimer)	//If timer has reached phoneTimer
            {
                callActive = true;		//There's a call
				if (!source.isPlaying)	//If there's no audio playing
					source.Play();		//Play the phone-ringing audio

				/*PHONE RINGING ANIMATION*/
                if (k % 2 == 1)
                {
                    randShake = (int)(Random.value * 3);
                    phone.transform.rotation = Quaternion.Euler(0, 0, randShake);
                    StartCoroutine(Stall());
					k = 0; //Prevents k from growing massive, but is otherwise useless
                }

                else if (k % 2 == 0)
                {
                    randShake = (int)(Random.value * 3);
                    phone.transform.rotation = Quaternion.Euler(0, 0, -(randShake));
                    StartCoroutine(Stall());
                }
                k++;
				/***************/
            }
        }
    }
	//This is a function for a button you press to skip to the next text
    public void buttonPush()
    {
        j++;
        if (j >= OpeningText.Length-1)
        {
            timer = 0;
			if (gameObject.GetComponent<Morals> ().level == 1)
				shiftLength = 60;
			else if (gameObject.GetComponent<Morals> ().level == 2)
				shiftLength = 120;
			else
				shiftLength = 180;
            phoneCollider = phone.GetComponent<Collider2D>();
            phoneCollider.enabled = true;
        }
    }
	//This is a function that skips forward in the opening text
	public void SkipToNextText()
	{
		StopAllCoroutines();
		currentlyDisplayingText++;
		if (currentlyDisplayingText > OpeningText.Length)
		{
			currentlyDisplayingText = 0;
		}
		StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));
	}
	//This is a function that removes the button...... duh.
    public void removeButton()
    {
        button.SetActive(false);
    }
	/************************************************************************
	 * public void phoneUp() is a function that:
	 * 	1.) Checks if the current call is the specific call
	 * 		a.) If it is, the randomPlug is set (to 0?) and the specific
	 * 			message is displayed. Then, specificBool is set to true.
	 * 		b.) If it isn't, randomPlug and linguistics are set to random 
	 * 			numbers between 1 and 10 inclusive, then based on the value
	 * 			of linguistics, concatenates a random phone number or
	 * 			location string to the end of the PhoneText at callNumber.
	 * 			This is displayed, the callNumber an
	 * 
	************************************************************************/

    public void phoneUp()
    {
        if (S == specificCall)
        {

            specificBool = true;
            happyTimer = 10;
            randPlug = specificPlug;
            PhoneText[specificCall] = specificMessage;
            StartCoroutine(AnimateText(PhoneText, specificCall));
<<<<<<< HEAD
            callActive = false;
            callNumber++;
            S++;
=======
            specificBool = true;
>>>>>>> origin/Allan's-Version
        }
        else
        {
            happyTimer = 10;
            randPlug = (int)(Random.value * 10);
            linguistics = (int)(Random.value * 10);

            if (linguistics % 2 == 1)
            {
                PhoneText[callNumber] = PhoneText[callNumber] + PhoneNumbers[randPlug];
            }
            else if (linguistics % 2 == 0)
            {
                PhoneText[callNumber] = PhoneText[callNumber] + Locations[randPlug];
            }
            StartCoroutine(AnimateText(PhoneText, callNumber));
        }
		callNumber++;
		S++;
		callActive = false;
    }

	/*COROUTINES*/
	IEnumerator AnimateText(string[] textArray, int call)
	{
		for (i = 0; i <= (textArray[call].Length); i++)
		{
			dialogue.text = textArray[call].Substring(0, i);
			yield return new WaitForSeconds(.03f);	//Slows the text scroll. Change .03f to a public float in order to change based on caller.
		}
	}
    IEnumerator Stall ()
    {
        yield return new WaitForSeconds(.1f);
    }
    
}
