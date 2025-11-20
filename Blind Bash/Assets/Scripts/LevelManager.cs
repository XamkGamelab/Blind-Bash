using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Prefabs")]
    public GameObject[] levelPrefabs;

    [Header("References")]
    public Transform levelParent;
    public Transform player;

    private GameObject currentLevel;


    void Start()
    {
        int index = 0;
        if (GameManager.Instance != null)
        {
            index = GameManager.Instance.SelectedLevelIndex;
        }

        index = NormalizeIndex(index);
        Debug.Log("Loading level " + index);
        LoadLevel(index);
    }

    int NormalizeIndex(int index)
    {
        if (levelPrefabs == null || levelPrefabs.Length == 0) return 0;

        if (index < 0 || index >= levelPrefabs.Length) return 0; //Fallback

        return index;
    }


    public void LoadLevel(int index)
    {
        //Destroy previous level if there is
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }

        //Instantiate new level
        currentLevel = Instantiate(levelPrefabs[index], levelParent);

        //Place player after level has been loaded
        StartCoroutine(PlacePlayerAtSpawnNextFrame());

    }

    IEnumerator PlacePlayerAtSpawnNextFrame()
    {
        //Wait for other start/awake to be finsihed
        yield return null;

        if (player == null)
        {
            Debug.LogWarning("Player reference not set on LevelManager");
            yield break;
        }

        //Find spawn by tag
        Transform spawn = null;
        foreach (var t in currentLevel.GetComponentsInChildren<Transform>(true))
        {
            if (t.CompareTag("SpawnPoint"))
            {
                spawn = t;
                break;
            }
        }

        if (spawn == null)
        {
            foreach (var t in currentLevel.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "SpawnPoint")
                {
                    spawn = t;
                    break;
                }
            }
        }

        if (spawn == null)
        {
            Debug.LogWarning("No spawn found");
            yield break;
        }

        //Place player
        player.position = spawn.position;
        player.rotation = spawn.rotation;

        Debug.Log("Player placed at " + spawn.position);
    }
}
