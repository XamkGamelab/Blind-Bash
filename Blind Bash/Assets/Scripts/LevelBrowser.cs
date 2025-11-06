using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelBrowser : MonoBehaviour
{
    public GameObject buttonPrefab;

    public GameObject buttonParent;

    public int totalLevels = 2;


    private void OnEnable()
    {
        //Clear old buttons
        for (int j = buttonParent.transform.childCount - 1; j >= 0; j--)
        {
            Destroy(buttonParent.transform.GetChild(j).gameObject);
        }


        for (int i = 0; i < totalLevels; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            int index = i;
            int levelLabel = i + 1;
            newButton.GetComponent<LevelButton>().levelText.text = levelLabel.ToString();
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(index));
        }
    }

    private void SelectLevel(int index)
    {
        Debug.Log("Level button clicked" + (index + 1));
        GameManager.Instance.SetSelectedLevel(index);
        SceneManager.LoadScene("LevelScene");
    }
}
