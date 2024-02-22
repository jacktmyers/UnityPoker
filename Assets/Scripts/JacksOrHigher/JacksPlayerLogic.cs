using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacksPlayerLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public int Bet;
    public int Wallet;
    public int StartingWallet = 99999;
    public bool BetPlaced;
    public bool SelectedCards;
    //public JacksHand Hand;
    void Start()
    {
        Bet = 1;
        Wallet = StartingWallet;
        BetPlaced = false;
        SelectedCards = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeBet(){
        Bet = (Bet%5)+1;
        if (Bet > Wallet){
            Bet = 1;
        }
    }
    public void PlaceBet(){
        BetPlaced = true;
    }
    public void ConfirmSelection(){
        SelectedCards = true;
    }
}
