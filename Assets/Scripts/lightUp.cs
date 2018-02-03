using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightUp : MonoBehaviour {

    public int lightNumber;	//The light's number
    public bool lightup;	//Whether the light is on or off

	//Function that fires when called with an integer lightNum
    public void OnCall(int lightNum)
    {
        if (lightNum == lightNumber)
        {//If the parameter is equal to this light's number, color it white, turning it on
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.color = Color.white;
        }

    }
	//Function that fires when the light is interrupted (i.e. when the cord is unplugged or the call is finished)
    public void Interrupted()
    {	//Color the light black, turning it off
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = Color.black;
    }
}