using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStuff : MonoBehaviour {
    public int overallScore = 0;
    public float happyTimer = 0;
    public int customersServed = 0;
    public int customersSatisfied = 0;

    public int i;
    private int j;
    public int k = 0;
    public int S;
    public GameObject button;
    public GameObject phone;
    public GameObject cord;
    public Text dialogue;
    public string[] OpeningText;
    public string[] PhoneText;
    public string[] PhoneNumbers;
    public string[] Locations;
    public int currentCall;
    public int randPlug;
    public bool callActive;
    public bool ye = true;
    private int randShake;
    private Collider2D phoneCollider;
    public int linguistics;

    /*TIME MECHANICS*/
    public float shiftLength;      //3 minutes of gameplay
    public float phoneTimer;       //New phonecall every 15 seconds (12 calls in first shift)
    public float timer = -11f;
    public int callNumber = 0;
    /****************/

        
    int currentlyDisplayingText = 0;
        void Awake()
        {
            StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));
        }
    private void Start()    //Initialize stuff here
    {
    }
    //This is a function for a button you press to skip to the next text
    public void SkipToNextText()
        {
            StopAllCoroutines();
            currentlyDisplayingText++;
            //If we've reached the end of the array, do anything you want. I just restart the example text
            if (currentlyDisplayingText > OpeningText.Length)
            {
                currentlyDisplayingText = 0;
            }
         StartCoroutine(AnimateText(OpeningText, currentlyDisplayingText));
    }
        //Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
        IEnumerator AnimateText(string[] textArray, int call)
        {
            for (i = 0; i <= (textArray[call].Length); i++)
            {
                dialogue.text = textArray[call].Substring(0, i);
                yield return new WaitForSeconds(.03f);
            }
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
            SceneManager.LoadScene("scoreboard 1");
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

                if (k % 2 == 1)
                {
                    randShake = (int)(Random.value * 3);
                    phone.transform.rotation = Quaternion.Euler(0, 0, randShake);
                    StartCoroutine(Stall());
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
    public void buttonPush()
    {
        j++;
        if (j >= 5)
        {
            timer = 0;
            shiftLength = 60;
            phoneCollider = phone.GetComponent<Collider2D>();
            phoneCollider.enabled = true;
        }
    }
    public void removeButton()
    {
        button.SetActive(false);
    }
    public void phoneUp()
    {
        /*if (S == 2)
        {
            happyTimer = 10;
            randPlug = 2;
            PhoneText[2] = "Hello this is bob";
            StartCoroutine(AnimateText(PhoneText, 2));
            callNumber++;
            callActive = false;
            cord.GetComponent<PickMeUp>().multiPlugged = false;
            S++;
        }
        else
        */
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
            cord.GetComponent<PickMeUp>().multiPlugged = false;
            S++;
        }
    }

    IEnumerator Stall ()
    {
        yield return new WaitForSeconds(.1f);
    }
    
}
