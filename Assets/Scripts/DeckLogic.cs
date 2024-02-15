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
        // Shuffle the Cards
        for (int c = 0; c < 52; c++)
        {
            int newLoc = Random.Range(0, 52);
            Card temp = Deck[c];
            Deck[c] = Deck[newLoc];
            Deck[newLoc] = temp;
        }
    }
         

    public Card? DrawCard()
    {
        Card? ret = null;
        if (Deck.Count >= 1)
        {
            ret = Deck[0];
            Deck.RemoveAt(0);
        }
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
