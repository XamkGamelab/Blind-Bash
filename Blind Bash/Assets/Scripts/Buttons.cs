using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void OnButtonPressed()
    {
        AudioManager.Instance.PlaySFX("bark");
        Invoke(nameof(StartGame), 0.15f);
    }

    public PauseManager PauseManager;
    public GameObject pauseCanvas;
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    public void QuiteGame()
    {
        Debug.Log("Game has been closed");
        Application.Quit();
    }
    public void Resume()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        PauseManager.isPaused = false;
    }
}
