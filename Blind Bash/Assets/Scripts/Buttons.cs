using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public PauseManager PauseManager;
    public GameObject pauseCanvas;

    public GameObject tutorialPanel;
    public GameObject creditsPanel;

    public void OnButtonPressed()
    {
        AudioManager.Instance.PlaySFX("bark");
        Invoke(nameof(StartGame), 0.15f);
    }

    public void StartGame()
    {
        Time.timeScale = 1f; //this here because this method is also used to go back to the levelselection after victory.
        SceneManager.LoadScene("LevelSelector"); //also for when player completes a level or leaves the current level.
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

    public void Restart()
    {
        Time.timeScale = 1f;

        FindObjectOfType<PlayerStats>().ResetStats(); //reset stats.

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene("Options");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
