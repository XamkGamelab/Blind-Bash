using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Prefabs")]
    public GameObject[] levelPrefabs;

    [Header("References")]
    public Transform levelParent;
    public CharacterMovement player;

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

        //Get config on the instantiated level
        LevelConfig config = currentLevel.GetComponent<LevelConfig>();
        if (config == null)
        {
            Debug.LogError("Level prefab is missing LevelConfig!");
            return;
        }

        //Place player at spawn
        player.transform.position = config.spawnPoint.position;
        player.transform.rotation = config.spawnPoint.rotation;

        //Tell player which tilemap to use for snapping
        player.SetLevelTilemap(config.collisionTilemap);

        Debug.Log("Player placed at" + config.spawnPoint.position);

    }
}
