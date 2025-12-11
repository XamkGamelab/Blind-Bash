using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelBrowser : MonoBehaviour
{
    public GameObject buttonPrefab;

    public GameObject buttonParent;

    public int totalLevels;


    private void OnEnable()
    {
        totalLevels = GameManager.Instance.totalLevels;
        //Clear old buttons
        for (int j = buttonParent.transform.childCount - 1; j >= 0; j--)
        {
            Destroy(buttonParent.transform.GetChild(j).gameObject);
        }

        int highestUnlocked = 0;
        if (GameManager.Instance != null)
        {
            highestUnlocked = GameManager.Instance.HighestUnlockedLevelIndex;
        }

        for (int i = 0; i < totalLevels; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            int index = i;
            int levelLabel = i + 1;

            LevelButton levelButton = newButton.GetComponent<LevelButton>();
            Button uiButton = newButton.GetComponent<Button>();

            //Set label text
            levelButton.levelText.text = levelLabel.ToString();

            //Determine if this level is unlocked
            bool isUnlocked = (i <= highestUnlocked);

            //Make locked button non-clickable
            uiButton.interactable = isUnlocked;

            //Update visuals
            levelButton.SetLocked(!isUnlocked);

            //Click listener
            uiButton.onClick.AddListener(() => SelectLevel(index));

        }
    }

    private void SelectLevel(int index)
    {
        Debug.Log("Level button clicked" + (index + 1));
        GameManager.Instance.SetSelectedLevel(index);
        SceneManager.LoadScene("LevelScene");
    }
}
