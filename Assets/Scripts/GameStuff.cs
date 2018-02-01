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
	 
    void Awake()
    {
        StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));	//Display opening text.
    }

    private void Start()    //Initialize stuff here
    {
        source = gameObject.GetComponent<AudioSource>();
    }
		
    public void Update()
    {
        if (j == (OpeningText.Length)-1)
            removeButton();


        /*TIME MECHANICS*/
        if (shiftLength > 0) {
            shiftLength -= Time.deltaTime;
        }
        else if (shiftLength < 0 && shiftLength >= -10) {
            //END THE DAY (THIS WORKS)
            Debug.Log("TIME'S UP");
            shiftLength = 0f;
            PlayerPrefs.SetInt("Served", customersServed);
            PlayerPrefs.SetInt("Satisfied", customersSatisfied);
            PlayerPrefs.SetInt("Overall", overallScore);
            if (gameObject.GetComponent<Morals>().level ==1)
            SceneManager.LoadScene("scoreboard 1");

            else if (gameObject.GetComponent<Morals>().level == 2)
                SceneManager.LoadScene("scoreboard 2");
        }

        if(happyTimer > 0)
        {
            happyTimer -= Time.deltaTime;
        }
        if (timer > -10)
        {
            timer += Time.deltaTime;
            if (timer >= phoneTimer)
            {
                callActive = true;
				if (!source.isPlaying)
					source.Play();

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


                /***************/
                k++;
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
    public void removeButton()
    {
        button.SetActive(false);
    }
    public void phoneUp()
    {
        if (S == specificCall)
        {

            specificBool = true;
            happyTimer = 10;
            randPlug = specificPlug;
            PhoneText[specificCall] = specificMessage;
            StartCoroutine(AnimateText(PhoneText, specificCall));
            callActive = false;
            callNumber++;
            S++;
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
            if (linguistics % 2 == 0)
            {
                PhoneText[callNumber] = PhoneText[callNumber] + Locations[randPlug];
            }
            StartCoroutine(AnimateText(PhoneText, callNumber));
            callNumber++;
            callActive = false;
            S++;
        }
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
