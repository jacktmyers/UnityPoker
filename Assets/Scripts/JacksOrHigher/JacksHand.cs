using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using static DeckLogic;

public class JacksHand : MonoBehaviour
{
    public List<GameObject> Hand;
    public List<float> CardLocs;
    public float CardSeparation = 1.5f;
    public GameObject Deck;
    // Start is called before the first frame update
    private int[] RoyalFlush = {
        250,500,750,1000,4000
    };
    private int[] StraightFlush = {
        50,100,150,200,250
    };
    private int[] FourOfAKind = {
        25,50,75,100,125
    };
    private int[] FullHouse = {
        9,18,27,36,45
    };
    private int[] Flush = {
        5,10,15,20,25
    };
    private int[] Straight = {
        4,8,12,16,20
    };
    private int[] ThreeOfAKind = {
        3,6,9,12,15
    };
    private int[] TwoPair = {
        2,4,6,8,10
    };
    private int[] JacksOrBetter = {
        1,2,3,4,5
    };
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCardToHand(GameObject cardGO, int? location = null){
        if (location == null){
            this.Hand.Add(cardGO);
        }
        else {
            this.Hand[(int)location] = cardGO;
        }
        cardGO.transform.parent = this.gameObject.transform;
        cardGO.GetComponent<JacksCardObject>().ParentHand = this;
        cardGO.transform.localRotation = quaternion.identity;
        CalculateNewCardLocations();
    }
    public void CalculateNewCardLocations(){
        CardLocs.Clear();
        float handWidth = CardSeparation * (Hand.Count - 1);
        float location = -1 * (handWidth / 2);
        foreach (GameObject card in Hand){
            Vector2 currCardLoc = new Vector2(location + this.transform.position.x, this.transform.position.y);
            card.GetComponent<JacksCardObject>().FinalRestingPlace = currCardLoc;
            CardLocs.Add(currCardLoc.x);
            location += CardSeparation;
        }
    }
    public void DiscardHand(){
        foreach(GameObject cardGO in Hand){
            Card curr = new Card(
                cardGO.GetComponent<JacksCardObject>().Suit,
                cardGO.GetComponent<JacksCardObject>().Number
            );
            cardGO.GetComponent<JacksCardObject>().DestroyOnArrival = true;
            cardGO.GetComponent<JacksCardObject>().FinalRestingPlace = Deck.transform.position;
        }
        Hand.Clear();
    }
    public void FillUnselected(DeckLogic deck, JacksCardObjectFactory jacksCardFactory){
        for (int i=0; i<Hand.Count; i++){
            if (!Hand[i].GetComponent<JacksCardObject>().Selected){
                GameObject discardCard = Hand[i];
                discardCard.GetComponent<JacksCardObject>().DestroyOnArrival = true;
                discardCard.GetComponent<JacksCardObject>().FinalRestingPlace = deck.transform.position;
                AddCardToHand(jacksCardFactory.CreateCardObject(deck.DrawCard()),i);
            }
            else {
                //Hand[i].GetComponent<JacksCardObject>().Selected = false;
            }
        }
    }
    public bool CheckForFlush(){
        char currSuit = Hand[0].GetComponent<JacksCardObject>().Suit;
        bool ret = true;
        for (int i=1; i<Hand.Count; i++){
            if (Hand[i].GetComponent<JacksCardObject>().Suit != currSuit){
                ret = false;
                break;
            }
        }
        return ret;
    }
    public bool CheckForRoyal(List<int> sortedCardNums){
        if (sortedCardNums[0] != 0)
            return false;
        if (sortedCardNums[1] != 9)
            return false;
        if (sortedCardNums[2] != 10)
            return false;
        if (sortedCardNums[3] != 11)
            return false;
        if (sortedCardNums[4] != 12)
            return false;
        return true;
    }
    public bool CheckForInOrder(List<int> sortedCardNums){
        int nextNum = sortedCardNums[0] +1;
        for (int i=1; i<sortedCardNums.Count; i++){
            if (sortedCardNums[i] == nextNum){
                nextNum = sortedCardNums[i] +1;
            }
            else{
                return false;
            }
        }
        return true;
    }
    public bool CheckForStraight(List<int> sortedCardNums){
        if (CheckForInOrder(sortedCardNums)){
            return true;
        }
        else if (sortedCardNums.Contains(0)){
            // Deep Copy
            List<int> tempCardNums = sortedCardNums.Select(x => x).ToList();
            tempCardNums.Remove(0);
            tempCardNums.Add(13);
            return CheckForInOrder(tempCardNums);
        }
        return false;
    }
    public List<(int,int)> ExtractMatches(List<int> sortedCardNums){
        List<(int,int)> ret = new List<(int,int)>();
        int currCount = 1;
        int? matchNum = null;
        foreach (int num in sortedCardNums){
            if (matchNum == null){
                matchNum = num;
            }
            else if (matchNum == num){
                currCount++;
            }
            else{
                if (currCount > 1){
                    ret.Add(((int)matchNum,currCount));
                }
                currCount = 1;
                matchNum = num;
            }
        }
        if (currCount > 1){
            ret.Add(((int)matchNum,currCount));
        }
        return ret;
    }
    public bool CheckForFullHouse(List<(int,int)> matches){
        if (matches.Count < 2){
            return false;
        }
        if ((matches[0].Item2+matches[1].Item2) != 5){
            return false;
        }
        return true;
    }
    public bool CheckForFourOfAKind(List<(int,int)> matches){
        if ((matches.Count == 1) && (matches[0].Item2 == 4)){
            return true;
        }
        return false;
    }
    public bool CheckForThreeOfAKind(List<(int,int)> matches){
        foreach ((int,int) match in matches){
            if (match.Item2 == 3)
                return true;
        }
        return false;
    }
    public bool CheckForTwoPairs(List<(int,int)> matches){
        int pairs = 0;
        foreach((int,int) match in matches){
            if (match.Item2 == 2){
                pairs++;
            }
        }
        return pairs == 2;
    }
    public bool CheckForJacksOrHigher(){
        foreach (GameObject cardGo in Hand){
            if ((int)cardGo.GetComponent<JacksCardObject>().Number > 9){
                continue;
            }
            if ((int)cardGo.GetComponent<JacksCardObject>().Number == 0){
                continue;
            }
            return false;
        }
        return true;
    }
    public (string,int) CalculateWinnings(int bet){
        bool flushBool = CheckForFlush();
        bool jacksBool = CheckForJacksOrHigher();

        List<int> sortedCardNums = Hand.Select(x => (int)x.GetComponent<JacksCardObject>().Number).ToList();
        sortedCardNums.Sort(); // Check if this is in descending order

        bool royalBool = CheckForRoyal(sortedCardNums);
        bool straightBool = CheckForStraight(sortedCardNums);

        List<(int,int)> matches = ExtractMatches(sortedCardNums);

        bool fullHouseBool = CheckForFullHouse(matches);
        bool fourOfAKindBool = CheckForFourOfAKind(matches);
        bool threeOfAKindBool = CheckForThreeOfAKind(matches);
        bool twoPairBool = CheckForTwoPairs(matches);
        
        if (royalBool && flushBool)
            return ("Royal Flush!",RoyalFlush[bet-1]);
        if (straightBool && flushBool)
            return ("Straight Flush!",StraightFlush[bet-1]);
        if (fourOfAKindBool)
            return ("Four Of A Kind!",FourOfAKind[bet-1]);
        if (fullHouseBool)
            return ("Full House!", FullHouse[bet-1]);
        if (flushBool)
            return ("Flush!", Flush[bet-1]);
        if (straightBool)
            return ("Straight!", Straight[bet-1]);
        if (threeOfAKindBool)
            return ("Three Of A Kind!", ThreeOfAKind[bet-1]);
        if (twoPairBool)
            return ("Two Pairs!",TwoPair[bet-1]);
        if (jacksBool)
            return ("Jacks Or Higher!", JacksOrBetter[bet-1]);
        return ("Nothing", 0);
    }
    public void SetHandUnmodifiable(){
        foreach (JacksCardObject card in Hand.Select(x => x.GetComponent<JacksCardObject>())){
            card.Selected = false;
            card.Modifiable = false;
        }
    }
}
