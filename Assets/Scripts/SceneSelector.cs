using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour {

	private int nextLevel;
	private Scene currentLevel;


	private void Start(){
		currentLevel = SceneManager.GetActiveScene ();
	}
    public void NextDay()
    {
		if (currentLevel.name == "Menu")
			nextLevel = 1;
		else
			nextLevel = PlayerPrefs.GetInt ("Next Level");
		
		PlayerPrefs.SetInt ("Next Level", nextLevel);
		SceneManager.LoadScene ("Level " + nextLevel);
    }
}
