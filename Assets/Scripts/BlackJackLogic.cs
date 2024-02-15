using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static DeckLogic;

public class BlackJackLogic : MonoBehaviour
{
    public List<GameObject> Players;
    public DeckLogic Deck;
    public CardObjectFactory CardFactory;
    public GameObject HitDecisionButtons;
    private enum GameState{
        DEALING,
        BETTING,
        HITTING,
    };
    private GameState CurrentState;
    public int PlayerTurn;
    // Start is called before the first frame update
    void Start()
    {
        Deck.CreateDeck();
        CurrentState = GameState.DEALING;
        HitDecisionButtons.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(CurrentState){
            case GameState.DEALING:
                DrawNewHands();
                CurrentState = GameState.HITTING;
                PlayerTurn = 0;
                break;
            case GameState.HITTING:
                if (PlayerTurn == Players.Count){
                    CurrentState = GameState.HITTING;
                    PlayerTurn = 0;
                }
                while(true){
                    try{
                        CPULogic cpu = Players[PlayerTurn].gameObject.GetComponent<CPULogic>();
                        if(cpu.DecideToHit())
                            HitMe(Players[PlayerTurn].gameObject.GetComponent<BlackJackHand>());
                        PlayerTurn++;
                    }
                    catch{
                        HitDecisionButtons.gameObject.SetActive(true);
                        break;
                    }
                }
                break;
                
        }
        
    }
    public void DrawNewHands(){
        for (int i = 0; i < 2; i++){
            foreach (BlackJackHand hand in Players.Select(x => x.GetComponent<BlackJackHand>())){
                hand.AddCardToHand(CardFactory.CreateCardObject((Card)Deck.DrawCard()));
            }
        }
    }
    public void HitMe(BlackJackHand targetHand){
        targetHand.AddCardToHand(CardFactory.CreateCardObject((Card)Deck.DrawCard()));
    }
    public void PlayerDecsion(bool hit){
        if (hit){
            HitMe(Players[PlayerTurn].gameObject.GetComponent<BlackJackHand>());
        }
        HitDecisionButtons.gameObject.SetActive(false);
        PlayerTurn++;
    }
}
