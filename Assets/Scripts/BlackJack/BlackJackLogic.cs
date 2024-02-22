using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using static DeckLogic;

public class BlackJackLogic : MonoBehaviour
{
    public GameObject Player;
    public GameObject Cpu;
    public DeckLogic Deck;
    public CardObjectFactory CardFactory;
    public GameObject HitDecisionButtons;
    public int GameEndTime;
    private System.DateTime? TimerEnd = null;
    private enum GameState{
        FIRSTBET,
        DEAL,
        SECONDBET,
        PLAYERHIT,
        CPUHIT,
        SETTLE,
        GAMEEND
    };
    public GameObject BetButton;
    public TextMeshProUGUI BetTextDisplay;
    public TextMeshProUGUI WalletTextDisplay;
    public TextMeshProUGUI PlayerScoreDisplay;
    public TextMeshProUGUI CpuScoreDisplay;
    public GameObject BetControls;
    public GameObject DoubleDecisionButtons;
    public TextMeshProUGUI SplashTextDisplay;
    private GameState CurrentState;
    // Start is called before the first frame update
    void Start()
    {
        Deck.CreateDeck();
        Deck.ShuffleDeck();
        CurrentState = GameState.FIRSTBET;
    }

    // Update is called once per frame
    void Update()
    {
        // Logic Updates
        BlackJackHand playerHand = Player.GetComponent<BlackJackHand>();
        BlackJackHand cpuHand = Cpu.GetComponent<BlackJackHand>();

        PlayerLogic playerLogic = Player.GetComponent<PlayerLogic>();
        CPULogic cpuLogic = Cpu.GetComponent<CPULogic>();
        // Game Logic State Machine
        switch(CurrentState){
            case GameState.FIRSTBET:
                if (playerLogic.BetPlaced){
                    playerLogic.BetPlaced = false;
                    CurrentState = GameState.DEAL;
                }
                break;
            case GameState.DEAL:
                for(int i=0; i<2; i++){
                    Player.GetComponent<BlackJackHand>()
                        .AddCardToHand(CardFactory.CreateCardObject(
                            Deck.DrawCard()
                        ));
                    Cpu.GetComponent<BlackJackHand>()
                        .AddCardToHand(CardFactory.CreateCardObject(
                            Deck.DrawCard(),
                            i == 0 // First card is face down
                        ));
                }
                playerLogic.CalculateTotalPoints();
                cpuLogic.CalculateTotalPoints();
                // Naturals check
                if ((cpuHand.TotalPointsHidden == 21) || (playerHand.TotalPointsHidden == 21)){
                    CurrentState = GameState.SETTLE;
                }
                CurrentState = GameState.SECONDBET;
                break;
            case GameState.SECONDBET:
                if (playerLogic.BetPlaced || (playerLogic.Wallet < playerLogic.Bet)){
                    playerLogic.BetPlaced = false;
                    CurrentState = GameState.PLAYERHIT;
                }
                break;
            case GameState.PLAYERHIT:
                if (playerHand.TotalPointsHidden > 21){
                    CurrentState = GameState.SETTLE;
                }
                else if (playerHand.TotalPointsHidden == 21){
                    CurrentState = GameState.CPUHIT;
                }
                else if (playerLogic.Pass){
                    playerLogic.Pass = false;
                    CurrentState = GameState.CPUHIT;
                }
                break;
            case GameState.CPUHIT:
                cpuLogic.DecideHits(CardFactory, Deck);
                CurrentState = GameState.SETTLE;
                break;
            case GameState.SETTLE:
                // Timer Logic
                cpuHand.UnhideCards();
                cpuLogic.CalculateTotalPoints();
                if (TimerEnd == null){
                    TimerEnd = System.DateTime.Now.AddSeconds(GameEndTime);
                    break;
                }
                else if (TimerEnd > System.DateTime.Now){
                    break;
                }
                TimerEnd = null;

                // Wallet Adjustment
                if (playerHand.TotalPointsHidden == cpuHand.TotalPointsHidden){
                    playerLogic.Wallet += playerLogic.Bet;
                }
                else if ((cpuHand.TotalPointsHidden > 21)&&(playerHand.TotalPointsHidden <= 21)){
                    playerLogic.Wallet += playerLogic.Bet * 2;
                }
                else if ((playerHand.TotalPointsHidden > cpuHand.TotalPointsHidden)&&(playerHand.TotalPointsHidden <= 21)){
                    playerLogic.Wallet += playerLogic.Bet * 2;
                }
                playerLogic.Bet = 0;

                // Discard Cards
                Deck.AddCardsToDiscard(playerHand.DiscardHand());
                playerLogic.CalculateTotalPoints();
                Deck.AddCardsToDiscard(cpuHand.DiscardHand());
                cpuLogic.CalculateTotalPoints();

                if (playerLogic.Wallet <= 0){
                    CurrentState = GameState.GAMEEND;
                }
                else {
                    CurrentState = GameState.FIRSTBET;
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

        // All Agnostic UI Updates
        WalletTextDisplay.text = $"$ {playerLogic.Wallet}";
        BetTextDisplay.text = playerLogic.Bet.ToString("D5");
        PlayerScoreDisplay.text = playerHand.TotalPointsVisible.ToString();
        CpuScoreDisplay.text = cpuHand.TotalPointsVisible.ToString();

        // Default Inactive State
        BetButton.SetActive(false);
        BetControls.SetActive(false);
        DoubleDecisionButtons.SetActive(false);
        HitDecisionButtons.SetActive(false);
        SplashTextDisplay.gameObject.SetActive(false);

        // UI Logic State Machine
        switch(CurrentState){
            case GameState.FIRSTBET:
                BetControls.SetActive(true);
                if (playerLogic.Bet > 0){
                    BetButton.SetActive(true);
                }
                break;
            case GameState.DEAL:
                break;
            case GameState.SECONDBET:
                if (playerLogic.Wallet >= playerLogic.Bet)
                    DoubleDecisionButtons.SetActive(true);
                break;
            case GameState.PLAYERHIT:
                HitDecisionButtons.SetActive(true);
                break;
            case GameState.CPUHIT:
                break;
            case GameState.SETTLE:
                SplashTextDisplay.gameObject.SetActive(true);
                // Wallet Adjustment
                if (playerHand.TotalPointsHidden == cpuHand.TotalPointsHidden){
                    SplashTextDisplay.text = "TIE!";
                }
                else if ((cpuHand.TotalPointsHidden > 21)&&(playerHand.TotalPointsHidden <= 21)){
                    SplashTextDisplay.text = "WIN!";
                }
                else if ((playerHand.TotalPointsHidden > cpuHand.TotalPointsHidden)&&(playerHand.TotalPointsHidden <= 21)){
                    SplashTextDisplay.text = "WIN!";
                }
                else{
                    SplashTextDisplay.text = "LOSS :(";
                }
               break;
            case GameState.GAMEEND:
                SplashTextDisplay.gameObject.SetActive(true);
                SplashTextDisplay.text = "GAME OVER!";
               break;
            default:
                Debug.LogError("GameState is not set to a valid state.");
                break;
        }
    }
}
