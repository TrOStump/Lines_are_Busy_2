using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightUp : MonoBehaviour {

    public int lightNumber;
    public bool lightup;
    private void Start()
    {
    }
    public void OnCall(int lightNum)
    {
        if (lightNum == lightNumber)
        {
            Debug.Log("lit)");
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            renderer.color = Color.white;
        }

    }
}