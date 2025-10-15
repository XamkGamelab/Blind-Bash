using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerHUD : MonoBehaviour
{
    [Header("TMP References")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>(); //ini. player stats for display use.
    }

    void Update()
    {
        if (playerStats == null) return;

        hpText.text = $"HP: {playerStats.hitPoints}";
        scoreText.text = $"Score: {playerStats.score}";
        movesText.text = $"Moves: {playerStats.moveCount}";
    }
}
