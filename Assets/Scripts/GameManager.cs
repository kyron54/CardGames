using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    HandEvaluator handEv;
    Hand hand;
    CardDeck deck;
    float waitTime = 2f;
    public int RoyalPay, StrFlushPay, FOAKPay, FullHousePay, FlushPay, StraightPay, TOAKPay, TwoPairPay, JOBPay;
    public int credits = 200;
    public int bet = 1;
    public int maxBet = 5;
    public TextMeshProUGUI winText, betText, creditsText, tableText;
    public List<GameObject> sortedHand = new List<GameObject>();
    public List<Button> buttonList = new List<Button>();

    public Button draw, deal, betButton, maxBetButton , betDown;

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.FindAnyObjectByType<Hand>().GetComponent<Hand>();
        deck = GameObject.FindAnyObjectByType<CardDeck>().GetComponent<CardDeck>();
        handEv = GameObject.FindAnyObjectByType<HandEvaluator>().GetComponent<HandEvaluator>();
    }

        // Update is called once per frame
    void Update()
    {
        UpdateText();
        UpdateTable();
        PayOut();
    }

    void UpdateText()
    {
        // Update Values tied to text.
        if (bet > maxBet)
        {
            bet = maxBet;
        }
        if(bet <= 0)
        {
            bet = 1;
        }

        if(credits <= 0)
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

        if (hand.hand.Count != 0)
        {
            hand.ClearHand();
        }
        hand.DealCards();

        credits = credits - bet;

        draw.gameObject.SetActive(true);
        deal.gameObject.SetActive(false);
        betButton.gameObject.SetActive(false);
        maxBetButton.gameObject.SetActive(false);
        betDown.gameObject.SetActive(false);

        StartCoroutine("ActivateButtons");

        winText.enabled = false;
    }

    public void Draw()
    {
        DeactivateButtons();

        if (hand.hand.Count != 0)
        {
            hand.RemoveCards();
        }
        hand.DealCards();

        StartCoroutine("HandRoutine");

        deck.ReshuffleDeck();

        deal.gameObject.SetActive(true);
        draw.gameObject.SetActive(false);
        betButton.gameObject.SetActive(true);
        maxBetButton.gameObject.SetActive(true);
        betDown.gameObject.SetActive(true);

        StartCoroutine("ActivateButtons");

        // Evaluates the hand.

    }

    void DeactivateButtons()
    {
        foreach (Button button in buttonList)
        {
            button.interactable = false;
        }
    }

    IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(waitTime);

        foreach (Button button in buttonList)
        {
            button.interactable = true;
        }
    }

    public void BetOne()
    {
        bet++;
    }

    public void MaxBet()
    {
        bet = maxBet;
    }

    public void BetDown()
    {
        bet--;
    }

    IEnumerator HandRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        sortedHand = handEv.SortHand(hand.hand);

        if (handEv.IsRoyalFlush(sortedHand))
        {
            Debug.Log("This hand is a Royal Flush");
        }

        if (handEv.IsStraight(sortedHand))
        {
            Debug.Log("This hand is a straight");
        }

        if (handEv.IsFlush(sortedHand))
        {
            Debug.Log("This hand is a flush");
        }

        handEv.EvaluateHand(sortedHand);
    }

    void UpdateTable()
    {
        tableText.text = "" + RoyalPay * bet + "\n" + StrFlushPay * bet + "\n" + FOAKPay * bet  + "\n" + FullHousePay * bet + "\n"
            + FlushPay * bet + "\n" + StraightPay * bet + "\n" + TOAKPay * bet + "\n" + TwoPairPay * bet + "\n" + JOBPay * bet;
    }

    void PayOut()
    {
        
    }

}
