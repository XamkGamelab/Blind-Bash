using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //player stats here.
    public int hitPoints = 3;
    public int score = 0;

    private GameObject ballParent;
    private GameObject gateSpriteRendererObject; //this simply for switching the sprites.
    public Sprite gateOpen;

    private SpriteRenderer gateSpriteRenderer;

    public int ballAmount;

    public bool canProceed = false;

    //private int totalLevels = 10;

    public 

    void Start()
    {
        StartCoroutine(WaitForParents()); //this for the reason that the game will find the gate and the balls.
    }

    IEnumerator WaitForParents()
    {
        while (ballParent == null)
        {
            ballParent = GameObject.Find("Balls");
            yield return null;
        }

        ballAmount = ballParent.transform.childCount;
        Debug.Log("found ball count = " + ballAmount);

        while(gateSpriteRenderer == null)
        {
            gateSpriteRendererObject = GameObject.FindGameObjectWithTag("Gate");

            gateSpriteRenderer = gateSpriteRendererObject.GetComponent<SpriteRenderer>();

            yield return null;
        }
    }

    void Update()
    {
        if (ballParent != null)
        {
            ballAmount = ballParent.transform.childCount;

            if (ballAmount == 0)
            {
                //Level progerssion logic goes HERE. Remember to reset score and HP.
                canProceed = true;

                Debug.Log("All balls collected.");
            }
        }

        //All balls have been collected so the next level can be unlocked
        if (canProceed)
        {
            ProceedLevel();
        }
    }


    public void AddScore(int amount) //score incrementation logic
    {
        score += amount;
    }

    public void TakeDamage(int amount) //HP decrementation logic
    {
        hitPoints -= amount; //decrement HP.
        if (hitPoints <= 0)
        {
            AudioManager.Instance.PlaySFX("lose");
            LevelUIManager.Instance.ShowLose(); //to show lose -canvas.

        }
    }

    public void ResetStats()
    {
        hitPoints = 3;
        score = 0;
    }

    public void ProceedLevel()
    {
        if (gateSpriteRenderer != null) 
        {
            gateSpriteRenderer.sprite = gateOpen;
        }
        int currentLevel = GameManager.Instance.SelectedLevelIndex;
        GameManager.Instance.UnlockNextLevel(currentLevel);

        Debug.Log("Level index " + (currentLevel + 1) + " opened");
    }
}
