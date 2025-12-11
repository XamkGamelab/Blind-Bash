using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    public GameObject winCanvas;
    public GameObject loseCanvas;

    void Awake()
    {
        Instance = this;
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    public void ShowWin()
    {
        winCanvas.SetActive(true);
        Time.timeScale = 0f; //freeze game
    }

    public void ShowLose()
    {
        loseCanvas.SetActive(true);
        Time.timeScale = 0f; //freeze game
    }
}
