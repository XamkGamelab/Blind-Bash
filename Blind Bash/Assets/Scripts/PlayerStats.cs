using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //player stats here.
    public int hitPoints = 3;
    public int score = 0;
    public int moveCount = 10;

    public void AddScore(int amount) //score incrementation logic
    {
        score += amount;
    }

    public void TakeDamage(int amount) //HP decrementation logic
    {
        hitPoints -= amount; //decrement HP.
        if (hitPoints <= 0)
        {
            //death logic to be implemented here
        }
        else
        {
            //something
        }
    }

    public void UseMove()
    {
        if (moveCount > 0)
        {
            moveCount--;
        }
        else
        {
            //something
        }
    }
}
