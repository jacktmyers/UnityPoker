using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class JacksLogic : MonoBehaviour
{
    private enum GameState{
        GAMESTART,
        BETTING,
        FIRSTDEAL,
        SELECTING,
        SECONDDEAL,
        SETTLE,
        SPLASH,
        GAMEEND,
    };
    private GameState CurrentState;
    public DeckLogic Deck;
    public GameObject Player;
    public JacksCardObjectFactory JacksCardFactory;
    public TextMeshProUGUI BetDisplay;
    public GameObject FirstDealButton;
    public GameObject ChangeBetButton;
    public GameObject SecondDealButton;
    private System.DateTime? TimerEnd;
    public TextMeshProUGUI Splash;
    public TextMeshProUGUI WalletDisplay;
    public int GameEndTime;
    // Start is called before the first frame update
    void Start()
    {
        CurrentState = GameState.GAMESTART;
    }

    // Update is called once per frame
    void Update()
    {
        JacksPlayerLogic playerLogic = Player.GetComponent<JacksPlayerLogic>();
        JacksHand playerHand = Player.GetComponent<JacksHand>();

        // Main Game Loop
        switch(CurrentState){
            case GameState.GAMESTART:
                if (playerLogic.Bet > playerLogic.Wallet){
                    playerLogic.Bet = playerLogic.Wallet;
                }
                Deck.DestroyDeck();
                Deck.CreateDeck();
                Deck.ShuffleDeck();
                CurrentState = GameState.BETTING;
                break;
            case GameState.BETTING:
                if (playerLogic.BetPlaced){
                    playerLogic.BetPlaced = false;
                    playerLogic.Wallet -= playerLogic.Bet;
                    CurrentState = GameState.FIRSTDEAL;
                }
                break;
            case GameState.FIRSTDEAL:
                for (int i=0; i<5; i++){
                    playerHand.AddCardToHand(JacksCardFactory.CreateCardObject(Deck.DrawCard()));
                    //playerHand.AddCardToHand(JacksCardFactory.CreateCardObject(Deck.DebugDrawCard()));
                }
                CurrentState = GameState.SELECTING;
                break;
            case GameState.SELECTING:
                if (playerLogic.SelectedCards){
                    playerLogic.SelectedCards = false;
                    CurrentState = GameState.SECONDDEAL;
                }
                break;
            case GameState.SECONDDEAL:
                playerHand.FillUnselected(Deck, JacksCardFactory);
                playerHand.SetHandUnmodifiable();
                CurrentState = GameState.SETTLE;
                break;
            case GameState.SETTLE:
                (string,int) betDescription = playerHand.CalculateWinnings(playerLogic.Bet);
                Splash.text = $"{betDescription.Item1}{(betDescription.Item2 > 0?$" +{betDescription.Item2}":"")}";
                playerLogic.Wallet += betDescription.Item2;
                CurrentState = GameState.SPLASH;
                break;
            case GameState.SPLASH:
                if (TimerEnd == null){
                    TimerEnd = System.DateTime.Now.AddSeconds(GameEndTime);
                    break;
                }
                else if (TimerEnd > System.DateTime.Now){
                    break;
                }
                TimerEnd = null;
                playerHand.DiscardHand();
                if (playerLogic.Wallet <= 0){
                    CurrentState = GameState.GAMEEND;
                }
                else{
                    CurrentState = GameState.GAMESTART;
                }
                break;
            case GameState.GAMEEND:
                if (TimerEnd == null){
                    TimerEnd = System.DateTime.Now.AddSeconds(GameEndTime);
                    break;
                }
                else if (TimerEnd > System.DateTime.Now){
                    break;
                }
                TimerEnd = null;
                SceneManager.LoadScene(0);
                break;
            default:
                Debug.LogError("GameState is not set to a valid state.");
                break;
        }
        // Agnostic UI Updates
        BetDisplay.text = $"BET: {playerLogic.Bet}";
        WalletDisplay.text = $"$ {playerLogic.Wallet}";
        // Default Off States
        FirstDealButton.SetActive(false);
        ChangeBetButton.SetActive(false);
        SecondDealButton.SetActive(false);
        Splash.gameObject.SetActive(false);
        // UI Updates
        switch(CurrentState){
            case GameState.GAMESTART:
                break;
            case GameState.BETTING:
                FirstDealButton.SetActive(true);
                ChangeBetButton.SetActive(true);
                break;
            case GameState.FIRSTDEAL:
                break;
            case GameState.SELECTING:
                SecondDealButton.SetActive(true);
                break;
            case GameState.SECONDDEAL:
                break;
            case GameState.SETTLE:
                break;
            case GameState.SPLASH:
                Splash.gameObject.SetActive(true);
                break;
            case GameState.GAMEEND:
                Splash.text = "GAME OVER!";
                Splash.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        
    }
}
