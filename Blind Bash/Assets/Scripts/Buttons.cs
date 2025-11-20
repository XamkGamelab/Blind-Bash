using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    public void QuiteGame()
    {
        Debug.Log("Game has been closed");
        Application.Quit();
    }
}
