using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    public Text scoreboard1;
    public Text scoreboard2;
    public Text scoreboard3;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        scoreboard1.text = ("Customers Served: " + PlayerPrefs.GetInt("Served"));
        scoreboard2.text = ("Customers Satisfied: " + PlayerPrefs.GetInt("Satisfied"));
        scoreboard1.text = ("Overall Score: " + PlayerPrefs.GetInt("Overall"));


    }
}
