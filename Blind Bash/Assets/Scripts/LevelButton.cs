using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public TMP_Text levelText;

    public GameObject lockIcon;

    //Update visuals for locked/unlocked state
    public void SetLocked(bool locked)
    {
        if (lockIcon != null)
        {
            lockIcon.SetActive(locked);
        }

        if (levelText != null)
        {
            var color = levelText.color;
            color.a = locked ? 0.4f : 1f;
            levelText.color = color;
        }
    }
}
