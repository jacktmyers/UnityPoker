using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static DeckLogic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BlackJackHand : MonoBehaviour
{
    public List<GameObject> Hand;
    public List<float> CardLocs;
    public float CardSeparation = 1.5f;
    public bool Modifiable = false;
    public int TotalPoints;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCardToHand(GameObject cardGO){
        this.Hand.Add(cardGO);
        cardGO.transform.parent = this.gameObject.transform;
        cardGO.GetComponent<CardObject>().ParentHand = this;
        cardGO.transform.localRotation = quaternion.identity;
        if (Modifiable)
            cardGO.GetComponent<CardObject>().Movable = true;
        CalculateNewCardLocations();
        CalculateTotalPoints();
    }
    public void CalculateNewCardLocations(){
        CardLocs.Clear();
        float handWidth = CardSeparation * (Hand.Count - 1);
        float location = -1 * (handWidth / 2);
        foreach (GameObject card in Hand){
            Vector2 currCardLoc = new Vector2(location, 0);
            card.transform.localPosition = currCardLoc;
            CardLocs.Add(card.transform.position.x);
            location += CardSeparation;
        }
    }
    public int GetClosestSlot(float dragCardX){
        float minDistance = Math.Abs(dragCardX - CardLocs[0]);
        int slotNum = 0;
        for (int i = 1; i < CardLocs.Count; i++){
            float currDistance = Math.Abs(dragCardX - CardLocs[i]);
            if (currDistance < minDistance){
                minDistance = currDistance;
                slotNum = i;
            }
            else{
                break;
            }
        }
        return slotNum;
    }
    public void MoveCardToSlot(GameObject cardGO, float cardX){
        int slot = GetClosestSlot(cardX);
        Hand.Remove(cardGO);
        Hand.Insert(slot,cardGO);
        for (int i = 0; i < Hand.Count; i++){
            Hand[i].GetComponent<CardObject>().FinalRestingPlace = 
                new Vector3(
                    CardLocs[i],
                    this.transform.position.y,
                    this.transform.position.z
                );
        }
    }
    public void CalculateTotalPoints(){
        TotalPoints = 0;
        foreach (GameObject card in Hand){
            TotalPoints += (int)card.GetComponent<CardObject>().Number;
        }
    }
}
