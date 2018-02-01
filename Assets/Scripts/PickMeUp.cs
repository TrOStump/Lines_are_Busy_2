using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickMeUp : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;
    public Collider2D plugCollider;
    private bool overPlug;
    public int plugNum;
    public float lightTimer = -11;
    private bool correctConnection = false;
    private int happyScore = 0;
    private int lightScore = 0;
	private int callerID = -1;

    /*PLUG CHOICE ASPECT*/
    public GameObject phone;
    public GameObject mainCam;
    /********************/

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
		Physics2D.IgnoreLayerCollision (8, 8, true);			//PREVENTS CORD COLLISION
		//plugOrder = new int[mainCam.GetComponent<GameStuff>().PhoneText.Length];
    }
    private void Update()
    {
		if (overPlug == true && lightTimer > -10)
			lightTimer -= Time.deltaTime;
		else if (overPlug == true && lightTimer <= -11) {
			
		}

        //If the plug is in AND the timer runs out, or if the timer is still active after the plug being removed
        if ((overPlug == true && lightTimer <= 0 && lightTimer > -10) || (overPlug == false && lightTimer > 0))
        {
			LightsOut ();	//Turns off any light with no cord in its respective plug

            if (lightTimer > 0)                                 //If the timer is still active (meaning the call wasn't finished)
            {
                correctConnection = false;                      //Customer is unhappy
                lightTimer = 0;
                if(mainCam.GetComponent<GameStuff>().specificBool)
                {
                    mainCam.GetComponent<GameStuff>().specificBool = false;
                    if(mainCam.GetComponent<Morals>().level == 2)
                        PlayerPrefs.SetInt("Police Contact",0);
                    else if((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 0))
                    {
                        SceneManager.LoadScene("Bad End");
                    }
                    else if ((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 1))
                    {
                        SceneManager.LoadScene("Mafia End");
                    }
                }
            }
            if (correctConnection && lightTimer > -10)                             //If customer is happy
            {
                lightScore += 10;                               //Score + 10
                Debug.Log("+10");
                if (mainCam.GetComponent<GameStuff>().specificBool && plugCollider.GetComponent<Number>().Num == mainCam.GetComponent<Morals>().correctPlug)
                {
                    if (mainCam.GetComponent<Morals>().level == 2)
                    {
                        PlayerPrefs.SetInt("Police Contact", 1);
                        Debug.Log("The police have been notified.");
                    }
                    else if ((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 1))
                    {
                        SceneManager.LoadScene("Police End");
                    }
                    else if ((mainCam.GetComponent<Morals>().level == 3) && (PlayerPrefs.GetInt("Police Contact") == 0))
                    {
                        SceneManager.LoadScene("meh End");
                    }
                    mainCam.GetComponent<GameStuff>().specificBool = false;
                }
                mainCam.GetComponent<GameStuff>().customersSatisfied++;
                lightTimer = -11;
				LightsOut ();
            }
            else if (!correctConnection && lightTimer > -10)
            {
                lightScore -= 5;
                Debug.Log("-5");
                lightTimer = -11;
            }
        }

        mainCam.GetComponent<GameStuff>().overallScore = happyScore + lightScore;
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        GetComponent<Animator>().SetBool("cordPluggedIn", false);

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint)+offset;
 		transform.position = curPosition;

    }

    private void OnMouseUp()
    {
        if (overPlug == true)
        {
            GetComponent<Animator>().SetBool("cordPluggedIn", true);

            transform.position = new Vector3((plugCollider.transform.position.x + .2f), (plugCollider.transform.position.y - 5.85f), -1);

            var lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject light in lights)
            {
                light.GetComponent<lightUp>().OnCall(plugCollider.GetComponent<Number>().Num);
            }


			if ((mainCam.GetComponent<GameStuff>().randPlug + 1) == plugCollider.GetComponent<Number>().Num && (callerID != mainCam.GetComponent<GameStuff>().callNumber))
            {
                happyScore = (int)mainCam.GetComponent<GameStuff>().happyTimer;
                mainCam.GetComponent<GameStuff>().overallScore += happyScore;
                mainCam.GetComponent<GameStuff>().customersServed++;
                Debug.Log(mainCam.GetComponent<GameStuff>().customersServed + " is how many customers were served.");
                lightTimer = 10;
                correctConnection = true;
				callerID = mainCam.GetComponent<GameStuff> ().callNumber;
            }
            if ((mainCam.GetComponent<GameStuff>().randPlug + 1) != plugCollider.GetComponent<Number>().Num)
            {
                lightTimer = 2;
                correctConnection = false;
                happyScore -= 2;
                Debug.Log("you aint my hommie :(");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plug"))
        {
            plugCollider = collision;
            overPlug = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plug"))
        {
            overPlug = false;
			LightsOut ();
        }
    }
	private void LightsOut()
	{
		var lights = GameObject.FindGameObjectsWithTag("light");
		foreach (GameObject light in lights)
		{//Checks all lights and turns them off ONLY if they're equal to the current plug collider's number
			if(light.GetComponent<lightUp>().lightNumber == plugCollider.GetComponent<Number>().Num){
				light.GetComponent<lightUp>().Interrupted();
				//Still turns it off while dragging one cord over another cord's current plug, SHOULD FIX LATER
			}
		}
	}
}
