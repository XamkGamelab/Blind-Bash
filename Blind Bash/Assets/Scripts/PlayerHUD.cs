using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public Text hpText;
    public Text scoreText;

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
    }
}
