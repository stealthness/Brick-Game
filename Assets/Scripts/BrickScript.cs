using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    public int MAX_NUMBER_HITS_TO_BREAK;
    public int MIN_NUMBER_HITS_TO_BREAK;
    public int points;
    public string[] powerUps;
    public int[] powerUpProbs;

    public int hitsToBreak;
    public string powerUp;

    public Sprite lightDamge;
    public Sprite heavyDamge;



    public void Start()
    {
        // Brick will be predefined powerup
        hitsToBreak = Random.Range(MIN_NUMBER_HITS_TO_BREAK, MAX_NUMBER_HITS_TO_BREAK+1);
        // set default powerup to to none
        powerUp = "none";
        if (powerUps.Length < 1)
        {
            //Debug.Log("No PowerUps");
            return;
        }
        else
        {
            // roll for each powerup in the list
            // this code can be improved later
            for (int i = 0; i < powerUps.Length; i++)
            {
                if (powerUpProbs[i] > Random.Range(0, 100))
                {
                    powerUp = powerUps[i];
                    break;
                }
            }
            
        }
        
    }


    internal void BreakBrick()
    {
        hitsToBreak--;
        if (hitsToBreak > 1)
        {
            GetComponent<SpriteRenderer>().sprite = lightDamge;
        }
        if (hitsToBreak == 1)
        {
            GetComponent<SpriteRenderer>().sprite = heavyDamge;
        }
        
    }
}
