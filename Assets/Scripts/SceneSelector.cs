using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour {

	private int nextLevel;
	private Scene currentLevel;


	private void Start(){
		currentLevel = SceneManager.GetActiveScene ();		//currentLevel is the current scene
	}
    public void NextDay()
    {
        if (currentLevel.name == "Menu") //If the current scene is the menu
            nextLevel = 1;					//The next level is Level 1
		else if (currentLevel.name == "end")
			nextLevel = -1;
        else//Otherwise, the next level is whatever the PlayerPrefs says it is
            nextLevel = PlayerPrefs.GetInt("Next Level");

		PlayerPrefs.SetInt ("Next Level", nextLevel);	//Next Level PLayerPref set
		if (nextLevel > 3)
			SceneManager.LoadScene ("end");					//Load end screen
		else if (nextLevel == -1)
			SceneManager.LoadScene ("Menu");
		else
			SceneManager.LoadScene ("Level " + nextLevel);	//Load the next level
    }
    public void Quit()
    {
        Application.Quit();
    }
}
