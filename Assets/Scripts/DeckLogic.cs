using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckLogic : MonoBehaviour
{
    public struct Card
    {
        public char Suit { get; }
        public char Number { get; }
        public Card(char s, char n)
        {
            Suit = s;
            Number = n;
        }
    }

    private List<Card> Deck = new List<Card>();
    private List<Card> Discard = new List<Card>();
    // Start is called before the first frame update
    void Start()
    {
    }
    public void CreateDeck()
    {
        // Initialize deck with all cards
        for (int s = 0; s < 4; s++)
        {
            for (int n = 0; n < 13; n++)
            {
                Card curr = new Card((char)s, (char)n);
                Deck.Add(curr);
            }
        }
    }
    public Card DrawCard()
    {
        Card ret;
        if (Deck.Count == 0)
        {
            TransferDiscard();
            ShuffleDeck();            
        }
        ret = Deck[0];
        Deck.RemoveAt(0);
        return ret;
    }

    public void ShuffleDeck(){
        // Shuffle the Cards
        for (int c = 0; c < Deck.Count; c++)
        {
            int newLoc = Random.Range(0, Deck.Count);
            Card temp = Deck[c];
            Deck[c] = Deck[newLoc];
            Deck[newLoc] = temp;
        }
    }

    public void TransferDiscard(){
        foreach (Card c in Discard){
            Deck.Add(c);
        }
        Discard.Clear();
    }

    public void AddCardsToDiscard(List<Card> discardedHand){
        foreach (Card card in discardedHand){
            Discard.Add(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
