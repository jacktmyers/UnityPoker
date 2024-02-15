using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DeckLogic;

public class TestGame : MonoBehaviour
{
    public DeckLogic Deck;
    public CardObjectFactory CardFactory;
    // Start is called before the first frame update
    void Start()
    {
        Deck.CreateDeck();
        CardFactory.CreateCardObject((Card)Deck.DrawCard());
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
