using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour {
    public string levelname;

    public void NextDay()
    {
        SceneManager.LoadScene(levelname);
    }
}
