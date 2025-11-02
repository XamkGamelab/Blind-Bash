using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int SelectedLevelIndex { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager created");
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
}
