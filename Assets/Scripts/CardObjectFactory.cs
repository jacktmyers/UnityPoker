using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DeckLogic;

public class CardObjectFactory : MonoBehaviour
{
    public GameObject CardPrefab;
    public Vector3 DefaultSpawnLocation;
    public List<Sprite> SuitOverlays;
    public List<Sprite> BlackNumberOverlays;
    public List<Sprite> RedNumberOverlays;
    public bool visible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreateCardObject(Card cardStruct, Vector3? spawnPoint = null)
    {
        if (spawnPoint == null)
            spawnPoint = DefaultSpawnLocation;
        GameObject cardGO = Instantiate(CardPrefab, (Vector3) spawnPoint, Quaternion.identity);
        cardGO.GetComponent<CardObject>().Suit = cardStruct.Suit;
        cardGO.GetComponent<CardObject>().Number = cardStruct.Number;

        // Set Suit and Number visual elements
        cardGO.transform.Find("SuitOverlay").GetComponent<SpriteRenderer>().sprite = SuitOverlays[(int)cardStruct.Suit];
        if (((int)cardStruct.Suit == 0) || ((int)cardStruct.Suit == 1)){
            cardGO.transform.Find("NumberOverlay").GetComponent<SpriteRenderer>().sprite = RedNumberOverlays[(int)cardStruct.Number];
        }
        else{
            cardGO.transform.Find("NumberOverlay").GetComponent<SpriteRenderer>().sprite = BlackNumberOverlays[(int)cardStruct.Number];
        }
        return cardGO;
    }

    public string GetSuitStringFromChar(char suit)
    {
        switch ((int)suit)
        {
            case 0:
                return "Hearts";
            case 1:
                return "Diamonds";
            case 2:
                return "Spades";
            case 3:
                return "Clubs";
            default:
                return "";
        }
    }
    public string GetNumberStringFromChar(char suit)
    {
        switch ((int)suit)
        {
            case 0:
                return "Ace";
            case 1:
                return "Two";
            case 2:
                return "Three";
            case 3:
                return "Four";
            case 4:
                return "Five";
            case 5:
                return "Six";
            case 6:
                return "Seven";
            case 7:
                return "Eight";
            case 8:
                return "Nine";
            case 9:
                return "Ten";
            case 10:
                return "Jack";
            case 11:
                return "Queen";
            case 12:
                return "King";
            default:
                return "";
        }
    }
}
