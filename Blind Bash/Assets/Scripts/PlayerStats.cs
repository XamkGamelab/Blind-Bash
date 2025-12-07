using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //player stats here.
    public int hitPoints = 3;
    public int score = 0;

    private GameObject ballParent;
    public int ballAmount;

    void Start()
    {
        StartCoroutine(WaitForBalls()); //this for the reason that the game won't find the "Balls" g.object instantly after instantiation.
    }

    IEnumerator WaitForBalls()
    {
        while (ballParent == null)
        {
            ballParent = GameObject.Find("Balls");
            yield return null;
        }

        ballAmount = ballParent.transform.childCount;
        Debug.Log("found ball count = " + ballAmount);
    }

    void Update()
    {
        if (ballParent != null)
        {
            ballAmount = ballParent.transform.childCount;

            if (ballAmount == 0)
            {
                //Level progerssion logic goes HERE. Remember to reset score and HP.

                Debug.Log("All balls collected.");
            }
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
            hitPoints = 3;
            score = 0;
            //reset stats on death. Here also logic to show "LOSE" view.

        }
    }
}
