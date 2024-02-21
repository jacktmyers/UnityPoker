using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CPULogic : MonoBehaviour
{
    public int DecisionThreshold = 17;
    public BlackJackHand Hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalculateTotalPoints(){
        Hand.TotalPointsHidden = 0;
        Hand.TotalPointsVisible = 0;
        foreach (GameObject card in Hand.Hand){
            if ((int)card.GetComponent<CardObject>().Number == 0){
               Hand.TotalPointsHidden += 11;
               if (!card.GetComponent<CardObject>().Hidden){
                    Hand.TotalPointsVisible += 11;
               }
            }
            else{
                Hand.TotalPointsHidden += card.GetComponent<CardObject>().PointValue;
               if (!card.GetComponent<CardObject>().Hidden){
                    Hand.TotalPointsVisible += card.GetComponent<CardObject>().PointValue;
               }
            }
        }
    }
    public void DecideHits(CardObjectFactory cardFactory, DeckLogic deck){
        while (true){
            if (Hand.TotalPointsHidden < DecisionThreshold){
                Hand.AddCardToHand(cardFactory.CreateCardObject(
                    deck.DrawCard()
                ));
                CalculateTotalPoints();
            }
            else {
                break;
            }
        }
    }
}
