using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMeUp : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;
    public Collider2D plugCollider;
    public bool overPlug;
    public int plugNum;
    private int happyScore;

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

                //ADD SCORE HERE
            }
            if ((mainCam.GetComponent<GameStuff>().randPlug + 1) != plugCollider.GetComponent<Number>().Num)
                Debug.Log("you aint my hommie :(");
            //Minus Score Here
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
        }
    }
}
