using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupSound : MonoBehaviour {

	public bool moving = false;
	public bool moved = false;
	public int random = 0;
	int arbit = 750;
	public int counter = 0;
	public AudioClip cupMove;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/***************
		 * 
		 * Pseudocode:
		 * 	Wait a certain amount of time so that the scare doesn't happen immediately.
		 * 	After timer:
		 * 		Randomize a number.
		 * 		If that number is equal to an arbitrary value, and the cup has not finished moving yet,
		 * 			Make the movement noise and the cup is now moving.
		 * 		If the cup is moving but has not finished moving,
		 * 			and If the cup hasn't moved past (arbitrary x position)
		 * 				Move the cup slightly towards (arbitrary x position)
		 * 			or If the cup hasn't moved past (farther x position)
		 * 				Move the cup even more slightly towards (farther x position) for an ease-in effect
		 * 			or If the cup has moved past (farther x position)
		 * 				The cup has finished moving
		 * 
		 ***************/
		if (counter <= 2000)
			counter++;
		else {
			random = (int)(Random.Range (0f, 1500f));
			if ((random == arbit) && (moved == false)) {
				gameObject.GetComponent<AudioSource> ().PlayOneShot (cupMove);
				moving = true;
			}
			if ((moving == true) && (moved == false)) {
				if(gameObject.GetComponent<Transform> ().position.x < 5.88)
					gameObject.GetComponent<Transform> ().Translate (new Vector3 (0.05f, 0f, 0f));
				else if ((moving == true) && (moved == false) && (gameObject.GetComponent<Transform> ().position.x < 5.98))
					gameObject.GetComponent<Transform> ().Translate (new Vector3 (0.02f, 0f, 0f));
				else if (gameObject.GetComponent<Transform> ().position.x > 5.98) {
					moving = false;
					moved = true;
				} 
			}
		}
	}
}
