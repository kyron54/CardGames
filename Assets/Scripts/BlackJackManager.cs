using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlackJackManager : MonoBehaviour
{
    HandEvaluator handEv;
    Hand hand;
    DealerHand dealer;
    CardDeck deck;
    float waitTime = 2f;
    public int bet = 10;
    public int credits = 1000;
    public TextMeshProUGUI winText, betText, creditsText;
    public List<GameObject> sortedHand = new List<GameObject>();
    public List<Button> buttonList = new List<Button>();

    public Button hit, deal, betButton, betDown, stand, doubleDown;

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.FindAnyObjectByType<Hand>().GetComponent<Hand>();
        dealer = GameObject.FindAnyObjectByType<DealerHand>().GetComponent<DealerHand>();
        deck = GameObject.FindAnyObjectByType<CardDeck>().GetComponent<CardDeck>();
        handEv = GameObject.FindAnyObjectByType<HandEvaluator>().GetComponent<HandEvaluator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        // Update Values tied to text.
        if (bet > credits)
        {
            bet = credits;
        }
        if (bet <= 0)
        {
            bet = 10;
        }

        if (credits <= 0)
        {
            DeactivateButtons();
        }

        // Update text.
        betText.text = "Bet: " + bet;

        creditsText.text = "Credits: " + credits;
    }

    // Button Functions

    public void Deal()
    {
        DeactivateButtons();
        if(doubleDown.interactable == false)
        {
            doubleDown.interactable = true;
        }

        if (hand.hand.Count != 0)
        {
            hand.ClearHand();
        }
        hand.PullOneCard();
        hand.PullOneCard();

        SwitchButtons();

        dealer.PullOneCard();

        // Evaluates the hand.
        StartCoroutine("HandRoutine");

        StartCoroutine(ToggleDoubleDown(true));

        StartCoroutine("ActivateButtons");
    }

    public void Hit()
    {
        DeactivateButtons();
        doubleDown.interactable = false;
        StartCoroutine(ToggleDoubleDown(false));

        hand.PullOneCard();

        // Evaluates the hand.
        StartCoroutine("HandRoutine");
    }

    public void DoubleDown()
    {
        bet *= 2;
        hand.PullOneCard();

        winText.text = "2x Bet!";
        doubleDown.interactable = false;

        StartCoroutine("CheckStand");
    }

    public void Stand()
    {
        StartCoroutine(CheckStand());
    }

    public void BetOne()
    {
        bet = bet + 10;
    }

    public void MaxBet()
    {
        bet = credits;
    }

    public void BetDown()
    {
        bet = bet - 10;
    }

    IEnumerator HandRoutine()
    {
        StartCoroutine("ActivateButtons");

        yield return new WaitForSeconds(waitTime);

        if(handEv.AddHandTotal(hand.hand) > 21)
        {
            DeactivateButtons();
            doubleDown.interactable = false;

            Bust(bet);

            winText.text = "Bust.";

            yield return new WaitForSeconds(waitTime);

            EndRound();
        }
        else if(handEv.AddHandTotal(hand.hand) == 21 && hand.hand.Count == 2)
        {
            DeactivateButtons();

            BlackJack(bet);
            winText.text = "BlackJack.";

            yield return new WaitForSeconds(waitTime);

            EndRound();
        }

        Debug.Log(handEv.AddHandTotal(hand.hand));
    }

    IEnumerator CheckStand()
    {
        DeactivateButtons();
        doubleDown.interactable = false;

        dealer.PullOneCard();

        yield return new WaitForSeconds(waitTime);

        int playerTotal = handEv.AddHandTotal(hand.hand);
        int dealerTotal = handEv.AddHandTotal(dealer.hand);

        if(playerTotal == 21 && dealerTotal == 21)
        {
            // no payout. Tie.
            winText.text = "Tie. Bets Returned.";

            yield return new WaitForSeconds(waitTime);
        }
        else if(playerTotal == 21 && dealerTotal != 21)
        {
            // Win bet
            PayOut(bet);
            winText.text = "Win.";
        }
        else if(dealerTotal < 21)
        {
            while(dealerTotal < 17)
            {
                if (dealerTotal <= playerTotal)
                {
                    dealer.PullOneCard();
                }
                else if(dealerTotal > playerTotal)
                {
                    // lose bet.
                    Bust(bet);
                    winText.text = "Dealer Wins.";

                    break;
                }

                yield return new WaitForSeconds(.5f);

                dealerTotal = handEv.AddHandTotal(dealer.hand);
            }
        }

        if (playerTotal > 21)
        {
            // lose bet.
            Bust(bet);
            winText.text = "Bust";
        }
        else if (dealerTotal > 21)
        {
            // Dealer Bust
            winText.text = "Win.";
            PayOut(bet);
        }
        else if(dealerTotal == playerTotal)
        {
            // no payout. Tie.
            winText.text = "Tie. Bets Returned.";
        }
        else if(dealerTotal > playerTotal)
        {
            // lose bet.
            Bust(bet);
            winText.text = "Dealer Wins.";
        }
        else if(dealerTotal < playerTotal)
        {
            // Win bet
            PayOut(bet);
            winText.text = "Win.";
        }

        yield return new WaitForSeconds(waitTime);

        Debug.Log("Dealer Total: " + dealerTotal + " Player Total: " + playerTotal);

        EndRound();
    }

    void Bust(int bet_)
    {
        credits = credits - bet_;
    }

    void BlackJack(int bet_)
    {
        credits = (int)(credits + (bet_ * 1.5f));
    }

    void PayOut(int bet_)
    {
        credits = credits + bet;
    }

    void EndRound()
    {
        //Reshuffle Deck
        deck.ReshuffleDeck();

        // Clear Variables
        hand.ClearHand();
        dealer.ClearHand();
        winText.text = "";
        bet = 0;

        StartCoroutine("ActivateButtons");
        StartCoroutine(ToggleDoubleDown(false));

        SwitchButtons();
    }

    // Button Functions

    void SwitchButtons()
    {
        foreach(Button button in buttonList)
        {
            button.gameObject.SetActive(!button.IsActive());
        }
    }

    void DeactivateButtons()
    {
        foreach (Button button in buttonList)
        {
            button.interactable = false;
        }
    }

    IEnumerator ToggleDoubleDown(bool isToggled)
    {
        yield return new WaitForSeconds(waitTime);

        doubleDown.gameObject.SetActive(isToggled);
    }

    IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(waitTime);

        foreach (Button button in buttonList)
        {
            button.interactable = true;
        }

    }
}
