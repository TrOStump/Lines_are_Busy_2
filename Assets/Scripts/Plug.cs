using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour {

    public int Num;
    public Collider2D cordCollider;
    public bool cordPlugged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cord"))
        {
            if (cordPlugged == false)
            {
                cordCollider = collision;
                cordCollider.GetComponent<PickMeUp>().overPlug = true;
            }
        }
    }
}
