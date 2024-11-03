using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public static Scoring s;
    // Start is called before the first frame update
    void Start()
    {
        s = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetScore()
    {
        int score = 0;

        int count = 0;
        // Score per item

        foreach (Collider c in Physics.OverlapBox(transform.position, transform.localScale))
        {
            Grabbable g = c.GetComponent<Grabbable>();
            if (g != null)
            {
                score += g.pointValue;
                count++;
            }
        }

        // Bonus for % of items saved
        float percentSaved = count / GameManager.S.currentCustomer.items.Count;
        
        if (percentSaved > 0.5) {
            score += (int)(Mathf.Floor(percentSaved * 10)) * 100;
        }

        if (percentSaved == 1)
        {
            score += 250;
        }

        // Time bonus

        // Not implemented

        Debug.Log(score);

        return score;
    }

}
