using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerLogic : MonoBehaviour
{
    //public List<Transform> CardSlots;
    //public TextMeshProUGUI PointDisplay;
    public bool BetPlaced;
    public bool Pass;
    public int Wallet;
    public int TempWallet;
    public int Bet;
    public BlackJackHand Hand;
    public CardObjectFactory CardFactory;
    public DeckLogic Deck;
    public int MaxBet = 99999;
    public int StartingWallet = 99999;
    // Start is called before the first frame update
    void Start()
    {
        Wallet = StartingWallet;
        Bet = 0;
        BetPlaced = false;
        Pass = false;
    }
    public void CalculateTotalPoints(){
        Hand.TotalPointsHidden = 0;
        int aces = 0;
        foreach (GameObject card in Hand.Hand){
            if ((int)card.GetComponent<CardObject>().Number == 0)
               aces++; 
            else
                Hand.TotalPointsHidden += card.GetComponent<CardObject>().PointValue;
        }
        bool min = true;
        for (int a = 0; a < aces; a++){
            int hypotheticalPoints = Hand.TotalPointsHidden+a+((aces - a)*11);
            if (hypotheticalPoints < 21){
                Hand.TotalPointsHidden = hypotheticalPoints;
                min = false; 
                break;
            }
        } 
        if (min){
            Hand.TotalPointsHidden += aces;
        }
        Hand.TotalPointsVisible = Hand.TotalPointsHidden;
    }
    public void UpdateBet(int place, bool increase){
        int tempBet = Bet;
        int placeMult = (int)Math.Pow(10,place);
        int oneGreaterMult = (int)Math.Pow(10,place+1);
        int digit = (Bet - (Bet/oneGreaterMult)*oneGreaterMult)/placeMult;
        if (increase){
            if(digit != 9)
                tempBet += placeMult;
        }
        else{
            if(digit != 0)
                tempBet -= placeMult;
        }
        if ((tempBet > Wallet) || (tempBet < 0) || (tempBet > MaxBet))
            return;
        Bet = tempBet;
        TempWallet = Wallet - Bet;
    }
    public void UpdateBet10K(bool increase){
        UpdateBet(4,increase);
    }
    public void UpdateBet1K(bool increase){
        UpdateBet(3,increase);
    }
    public void UpdateBet100(bool increase){
        UpdateBet(2,increase);
    }
    public void UpdateBet10(bool increase){
        UpdateBet(1,increase);
    }
    public void UpdateBet1(bool increase){
        UpdateBet(0,increase);
    }
    public void SubmitBet(){
        Wallet = TempWallet;
        BetPlaced = true;
    }
    public void SecondBetStay(){
        BetPlaced = true;
    }
    public void HitCard() {
        Hand.AddCardToHand(CardFactory.CreateCardObject(
            Deck.DrawCard()
        ));
        CalculateTotalPoints();
    }
    public void SecondBetDouble(){
        Wallet -= Bet;
        Bet = Bet*2;
        BetPlaced = true;
    }
    public void HitDice() {
        
    }
    public void SumbitPass(){
        Pass = true;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
