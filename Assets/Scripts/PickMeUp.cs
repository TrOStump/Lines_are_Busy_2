using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMeUp : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;
    public Collider2D plugCollider;
    public bool overPlug;
    public int plugNum;
    private float lightTimer = -11;
    private bool correctConnection = false;
    private int happyScore = 0;
    private int lightScore = 0;

    /*PLUG CHOICE ASPECT*/
    public GameObject phone;
    public GameObject mainCam;
    /********************/

    private void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        //plugOrder = new int[mainCam.GetComponent<GameStuff>().PhoneText.Length];
    }
    private void Update()
    {
        if (overPlug == true)
            lightTimer -= Time.deltaTime;

        //If the plug is in AND the timer runs out, or if the timer is still active after the plug being removed
        if ((overPlug == true && lightTimer <= 0 && lightTimer > -10) || (overPlug == false && lightTimer > 0))
        {
            if (lightTimer > 0)                                 //If the timer is still active (meaning the call wasn't finished)
            {
                correctConnection = false;                      //Customer is unhappy
                lightTimer = 0;
            }
            var lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject light in lights)
            {
                light.GetComponent<lightUp>().Interrupted();    //Turn off all lights
            }
            if (correctConnection && lightTimer > -10)                             //If customer is happy
            {
                lightScore += 10;                               //Score + 10
                Debug.Log("+10");
                lightTimer = -11;
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


            if ((mainCam.GetComponent<GameStuff>().randPlug + 1) == plugCollider.GetComponent<Number>().Num)
            {
                happyScore = (int)mainCam.GetComponent<GameStuff>().happyTimer;
                mainCam.GetComponent<GameStuff>().overallScore += happyScore;
                Debug.Log(mainCam.GetComponent<GameStuff>().overallScore + " is the player's current score.");
                lightTimer = 10;
                correctConnection = true;
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
            var lights = GameObject.FindGameObjectsWithTag("light");
            foreach (GameObject light in lights)
            {
                light.GetComponent<lightUp>().Interrupted();
            }
        }
    }
}
