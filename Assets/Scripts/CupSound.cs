using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupSound : MonoBehaviour {

	bool moving = false;
	bool moved = false;
	int random = 0;
	int arbit = 1000;
	public AudioClip cupMove;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		random = (int)(Random.Range(0f, 2000f));
		if((random==arbit)&&(moved==false))
		{
			gameObject.GetComponent<AudioSource>().PlayOneShot(cupMove);
			moving = true;
		}
		if ((moving == true) && (moved == false) && (gameObject.GetComponent<Transform> ().position.x < 4.85)) {
			gameObject.GetComponent<Transform> ().Translate (new Vector3 (0.05f, 0f, 0f));
		} else if ((moving == true) && (moved == false) && (gameObject.GetComponent<Transform> ().position.x < 5)) {
			gameObject.GetComponent<Transform> ().Translate (new Vector3 (0.02f, 0f, 0f));
		} else if (gameObject.GetComponent<Transform> ().position.x > 5) {
			moved = true;
		}
	}
}
