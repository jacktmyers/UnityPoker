using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacksOrHigherLogic : MonoBehaviour
{
    private int[] RoyalFlushLookup = {
        250,500,750,1000,4000
    };
    private enum GameState{
        BETTING,
        FIRSTDEAL,
        SELECTING,
        SECONDDEAL,
        SETTLE
    };
    private GameState CurrentState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(CurrentState){

        }
        
    }
}
