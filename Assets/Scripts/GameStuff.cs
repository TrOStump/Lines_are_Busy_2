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
    private int randShake;			//Random degree value of the phone's shaking
    private Collider2D phoneCollider;

    /*TIME MECHANICS*/
    public float shiftLength;      	//3 minutes of gameplay
    public float phoneTimer;       	//New phonecall happens every phoneTimer seconds.
    public float timer = -11f;		//timer counts upward until it equals phoneTimer, then the phone starts ringing.
    public int callNumber = 0;
    /****************/

    private void Start()    //Initialize stuff here
    {
        source = gameObject.GetComponent<AudioSource>();
		StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));	//Display opening text.
    }

    public void Update()	//Bulk of the coding
    {
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
				{	//If K is odd, shake to the right by randShake degrees
                    randShake = (int)(Random.value * 3);
                    phone.transform.rotation = Quaternion.Euler(0, 0, randShake);
					StartCoroutine(Stall());	//(pause very slightly)
					k = 0; //Prevents k from growing massive, but is otherwise useless
                }
                else if (k % 2 == 0)
                {	//If K is even, shake to the left by randShake degrees
                    randShake = (int)(Random.value * 3);
                    phone.transform.rotation = Quaternion.Euler(0, 0, -(randShake));
					StartCoroutine(Stall());	//(pause very slightly)
                }
                k++;
				/***************/
            }
        }
    }
	//This is a function for a button you press to skip to the next text
    public void buttonPush()
    {
        j++;	//increment the number of times the button has been pushed
        if (j >= OpeningText.Length-1)
		{//If it has been pushed one less time than the number of lines of dialogue in the opening text (or has been pushed more somehow)
            timer = 0;	//Timer start
			if (gameObject.GetComponent<Morals> ().level == 1)		//If level 1, shift is 1 minute long
				shiftLength = 60;
			else if (gameObject.GetComponent<Morals> ().level == 2)	//Else if level 2, shift is 2 minutes long
				shiftLength = 120;
			else 													//Else if any other level, shift is 3 minutes long
				shiftLength = 180;
            phoneCollider = phone.GetComponent<Collider2D>();		//Get the phone's hitbox
            phoneCollider.enabled = true;							//Turn on the phone's hitbox
			button.SetActive(false);								//Removes the button
        }
    }
	//This is a function that skips forward in the opening text
	public void SkipToNextText()
	{//Stops dialogue from scrolling, moves to the next line of dialogue, resets the line to the start, then displays that line
		StopAllCoroutines();
		currentlyDisplayingText++;
		if (currentlyDisplayingText > OpeningText.Length)
			currentlyDisplayingText = 0;
		StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));
	}

	/************************************************************************
	 * 
	 * public void phoneUp() is a function that:
	 * 	1.) Checks if the current call is the specific call
	 * 		a.) If it is, specificBool is set to true, randPlug is set to the
	 * 			specificPlug, and the specificCall starts displaying.
	 * 		b.) If it isn't, randomPlug and linguistics are set to random 
	 * 			numbers between 1 and 10 inclusive, then based on the value
	 * 			of linguistics, concatenates a random phone number or
	 * 			location string to the end of the PhoneText at callNumber.
	 * 			This is then displayed.
	 * 	2.)	Starts the happyTimer, increments the callNumber and S, and sets
	 * 		callActive to false (meaning that there is no call currently).
	 * 
	************************************************************************/
    public void phoneUp()
    {
        if (S == specificCall)
        {
            specificBool = true;
            randPlug = specificPlug;
            PhoneText[specificCall] = specificMessage;
            StartCoroutine(AnimateText(PhoneText, specificCall));
        }
        else
        {
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
		happyTimer = 10;
		callNumber++;
		S++;
		callActive = false;
    }

	/*COROUTINES*/
	//AnimateText animates the text by only displaying the substring from 0 to i, incrementing i after 0.03 seconds
	IEnumerator AnimateText(string[] textArray, int call)
	{
		for (i = 0; i <= (textArray[call].Length); i++)
		{
			dialogue.text = textArray[call].Substring(0, i);
			yield return new WaitForSeconds(.03f);	//Slows the text scroll. Change .03f to a public float in order to change based on caller.
		}
	}
	//Stall stalls the code for 0.1 seconds (used in the phone animation section for making sure the ringing is visible)
    IEnumerator Stall ()
    {
        yield return new WaitForSeconds(.1f);
    }
    
}
