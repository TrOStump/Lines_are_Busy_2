using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickMeUp : MonoBehaviour {
	/*Mouse-to-Cord Movement Mechanics*/
    private Vector3 screenPoint;
    private Vector3 offset;

	/*Score Mechanics*/
    private int happyScore = 0;
    private int lightScore = 0;

	/*Light Mechanics*/
	public float lightTimer = -11;
	private bool correctConnection = false;
	private int callerID = -1;

    /*Plug Choice Mechanics*/
	public Collider2D plugCollider;
	public bool overPlug;
	public int plugNum;
    public bool pluggedIn = false;

	/*Misc. Game Objects*/
    public GameObject phone;
    public GameObject mainCam;
    /********************/

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");				//Initializes the mainCam variable
		Physics2D.IgnoreLayerCollision (8, 8, true);			//PREVENTS CORD COLLISION
    }

    private void Update()
    {
		if (overPlug == true && lightTimer > -10)		//If cord is over a plug and the lightTimer is going
			lightTimer -= Time.deltaTime;				//Countdown

        if ((overPlug == true && lightTimer <= 0 && lightTimer > -10) || (overPlug == false && lightTimer > 0))
		{//If the plug is in AND the timer runs out, or if the timer is still active after the plug being removed
			LightsOut ();	//Turns off any light with no cord in its respective plug

            if (lightTimer > 0)
            {//If the call has started but not finished
                correctConnection = false;		//Wrong number
				lightTimer = 0;					//lightTimer set to 0 (NOT SENTINEL BECAUSE RESETTING THE CALL WOULD CHANGE THE OUTCOME)
                if(mainCam.GetComponent<GameStuff>().specificBool)
                {//If it's the specificCall
                    if(mainCam.GetComponent<Morals>().level == 2)	//If this is level 2, the police haven't been notified
                        PlayerPrefs.SetInt("Police Contact",0);
                    else if((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 1))
                    {//If this is level 3 and the police were contacted in level 2
                        SceneManager.LoadScene("Mafia End");
                    }
                    else if ((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 2))
                    {//If this is level 3 and the police were NOT contacted in level 2
                        SceneManager.LoadScene("Bad End");
                    }
					mainCam.GetComponent<GameStuff>().specificBool = false;	//Reset specificBool to false (so that this is no longer the specificCall)
                }
            }
            if (correctConnection && lightTimer > -10)
            {//If connection is right and the lightTimer has started
                lightScore += 10;	//Score + 10
                if (mainCam.GetComponent<GameStuff>().specificBool && plugCollider.GetComponent<Plug>().Num == mainCam.GetComponent<Morals>().correctPlug)
                {//If it's the specificCall and the plug is the correct plug
                    if (mainCam.GetComponent<Morals>().level == 2)
                    {//If this is level 2, then the police have been contacted
                        PlayerPrefs.SetInt("Police Contact", 1);
                        Debug.Log("The police have been notified.");
                    }
                    else if ((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 1))
                    {//If this is level 3 and the police were contacted in level 2
                        SceneManager.LoadScene("Police End");
                    }
                    else if ((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 0))
                    {//If this is level 3 and the police were NOT contacted in level 2
                        SceneManager.LoadScene("meh End");
                    }
					mainCam.GetComponent<GameStuff>().specificBool = false;	//Reset specificBool to false (so that this is no longer the specificCall)
                }
                mainCam.GetComponent<GameStuff>().customersSatisfied++;		//increment customersSatisfied
                lightTimer = -11;											//Reset lightTimer to sentinel
				LightsOut ();												//Turn off respective lights
            }
            else if (!correctConnection && lightTimer > -10)
            {//If connection is wrong and the lightTimer has started
				/*lightScore is decremented and the lightTimer is reset to the sentinel*/
				lightScore -= 5;
                lightTimer = -11;
            }
        }

        mainCam.GetComponent<GameStuff>().overallScore = happyScore + lightScore;	//update the score
    }

	//Function that fires on mouseclick
    void OnMouseDown()
    {
		/*Find the cord's position on the screen, set offset equal to the cord's world-point MINUS the mouse's world-point, then animate the cord being unplugged.*/
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        GetComponent<Animator>().SetBool("cordPluggedIn", false);

    }

	//Function that fires on mousedrag
    void OnMouseDrag()
    {
		/*Find the mouse position, find its relative world-point, set the cord's position equal to the mouse's relative world-point.*/
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint)+offset;
 		transform.position = curPosition;

    }

	//Function that fires on mouse release
    private void OnMouseUp()
    {
        if (overPlug == true)	//If the cord is over a plug
        {
			/*Snap the cord into the center position of the plug, then animate it being plugged in.*/
			transform.position = new Vector3((plugCollider.transform.position.x + .0f), (plugCollider.transform.position.y -.35f), -1);
            GetComponent<Animator>().SetBool("cordPluggedIn", true);

            var lights = GameObject.FindGameObjectsWithTag("light");			//Create an array of lights
            foreach (GameObject light in lights)								//For each element in the array of lights
                light.GetComponent<lightUp>().OnCall(plugCollider.GetComponent<Plug>().Num);	//If it's the correct light, turn it on.

			if ((plugCollider.GetComponent<Plug>().Num == mainCam.GetComponent<GameStuff>().randPlug + 1) && (mainCam.GetComponent<GameStuff>().callNumber) != callerID)
			{//If the plug's number is the same as the caller's random plug + 1, AND the current call number is not the caller ID
                happyScore += (int)mainCam.GetComponent<GameStuff>().happyTimer;//accumulate happyScore
                mainCam.GetComponent<GameStuff>().customersServed++;			//increment customer counter
				lightTimer = 10;												//Start happy light timer (light stays for 10 seconds)
                correctConnection = true;										//This IS the correct connection
				callerID = mainCam.GetComponent<GameStuff> ().callNumber;		//ID = current call number
            }
			else if (plugCollider.GetComponent<Plug>().Num != (mainCam.GetComponent<GameStuff>().randPlug + 1))
            {//If the plug's number isn't the caller's random plug number + 1
				lightTimer = 2;													//Start unhappy light timer (light stays for 2 seconds)
                correctConnection = false;										//This IS NOT the correct connection
				happyScore -= 2;												//decrement happyScore								
            }
            pluggedIn = true; // cord is plugged in
            plugCollider.GetComponent<Plug>().cordPlugged = true;
        }
    }

	//Function that fires upon trigger collider enter (PLUGS)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plug"))	//If the cord enters a plug's collider and is not in a cord's collider
        {
            if (collision.gameObject.GetComponent<Plug>().cordPlugged == false)
            {
                plugCollider = collision;                   //plugCollider is the current collider it is over
                overPlug = true;                            //The cord is now over a plug
            }
        }
    }

	//Function that fires upon trigger collider exit (PLUGS)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pluggedIn == true) // if the plugged in cord is being moved not another cord
        {
            if (collision.gameObject.CompareTag("Plug"))    //If the cord leaves the plug's collider
            {
                overPlug = false;                           //It is no longer over the plug
                LightsOut();                                //Turn off its respective light, if it is on
                pluggedIn = false;
                collision.gameObject.GetComponent<Plug>().cordPlugged = false;
            }
        }
    }

	//Fuction that turns off any lights that would otherwise be on
	private void LightsOut()
	{
		var lights = GameObject.FindGameObjectsWithTag("light");
		foreach (GameObject light in lights)
		{//Checks all lights and turns them off ONLY if they're equal to the current plug collider's number
			if(light.GetComponent<lightUp>().lightNumber == plugCollider.GetComponent<Plug>().Num){
				light.GetComponent<lightUp>().Interrupted();

				/*Still turns it off while dragging one cord over another cord's current plug, SHOULD FIX LATER*/

			}
		}
	}
}
