﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStuff : MonoBehaviour {
    public int i;
    private int j;
    public GameObject button;
    public GameObject phone;
    public Text dialogue;
    public string[] OpeningText;
    public string[] PhoneText;
    public string[] PhoneNumbers;
    public int currentCall;
    public int randPlug;

    /*TIME MECHANICS*/
    public float shiftLength;      //3 minutes of gameplay
    public float phoneTimer;       //New phonecall every 15 seconds (12 calls in first shift)
    public float timer = 0f;
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
        else if (shiftLength < 0f) {
            //END THE DAY (THIS WORKS)
            Debug.Log("TIME'S UP");
            shiftLength = 0f;
        }

        timer += Time.deltaTime;
        if (timer >= phoneTimer) {
            //Debug.Log("Rrrring, rrrring! BANANA PHONE~");
        }

        /***************/

    }
    public void buttonPush()
    {
        j++;
    }
    public void removeButton()
    {
        button.SetActive(false);
    }
    public void phoneUp()
    {
        randPlug = (int)(Random.value * 10);
        PhoneText[callNumber] = PhoneText[callNumber] + PhoneNumbers[randPlug];
        StartCoroutine(AnimateText(PhoneText, callNumber));
        callNumber++;
    }
    
}