using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int HighestUnlockedLevelIndex { get; private set; }

    private const string HighestUnlockedKey = "HighestUnlockedLevelIndex";

    public int SelectedLevelIndex { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager created");

            //Load highest unlocked level from PlayerPrefs
            HighestUnlockedLevelIndex = PlayerPrefs.GetInt(HighestUnlockedKey);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedLevel(int index)
    {
        SelectedLevelIndex = index;
        Debug.Log("Selected level index " + index);
    }

    //Call UnlockNextLevel when a level is completed to unlock the next one
    //completedIndex is the index of the level that was just finished
    public void UnlockNextLevel(int completedIndex, int totalLevels)
    {
        int nextIndex = completedIndex + 1;

        //Making sure we don't go past the last level
        if (nextIndex >= totalLevels)
        {
            return;
        }

        if (nextIndex > HighestUnlockedLevelIndex)
        {
            HighestUnlockedLevelIndex = nextIndex;
            PlayerPrefs.SetInt(HighestUnlockedKey, HighestUnlockedLevelIndex);
            PlayerPrefs.Save();
            Debug.Log("Unlocked level index " + nextIndex);
        }
    }
}
