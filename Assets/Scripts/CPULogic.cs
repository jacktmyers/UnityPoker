using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPULogic : MonoBehaviour
{
    public int DecisionThreshold = 18;
    public BlackJackHand Hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool DecideToHit(){
        if (Hand.TotalPoints < DecisionThreshold){
            return true;
        }
        return false;
    }
}
