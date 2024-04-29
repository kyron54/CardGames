using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class HandEvaluator : MonoBehaviour
{
    bool isFourKind = false, isFullHouse = false , isThreeKind = false, isTwoPair = false, isJacks = false;
    public TextMeshProUGUI winText;
    public GameManager gm;
    public BlackJackManager bm;

    private void Start()
    {

    }

    /// <summary>
    /// Evaluates the kind of poker hand the player has.
    /// </summary>
    /// <param name="hand">The hand to check.</param>
    public void EvaluateHand(List<GameObject> hand)
    {
        CheckPairs(hand);

        if(IsRoyalFlush(hand))
        {
            winText.text = "Royal Flush";
            winText.enabled = true;
            gm.credits += gm.bet * gm.RoyalPay;
        }
        else if(IsStraightFlush(hand))
        {
            winText.text = "Straight Flush";
            winText.enabled = true;
            gm.credits += gm.bet * gm.StrFlushPay;
        }
        else if(FourKind())
        {
            winText.text = "Four Of A Kind";
            winText.enabled = true;
            gm.credits += gm.bet * gm.FOAKPay;
        }
        else if(FullHouse())
        {
            winText.text = "Full House";
            winText.enabled = true;
            gm.credits += gm.bet * gm.FullHousePay;
        }
        else if(IsFlush(hand))
        {
            winText.text = "Flush";
            winText.enabled = true;
            gm.credits += gm.bet * gm.FlushPay;
        }
        else if(IsStraight(hand))
        {
            winText.text = "Straight";
            winText.enabled = true;
            gm.credits += gm.bet * gm.StraightPay;
        }
        else if(ThreeKind())
        {
            winText.text = "Three Of A Kind";
            winText.enabled = true;
            gm.credits += gm.bet * gm.TOAKPay;
        }
        else if(TwoPair())
        {
            winText.text = "Two Pairs";
            winText.enabled = true;
            gm.credits += gm.bet * gm.TwoPairPay;
        }
        else if(JacksHigher())
        {
            winText.text = "Jacks or Higher";
            winText.enabled = true;
            gm.credits += gm.bet * gm.JOBPay;
        }

    }

    public List<GameObject> SortHand(List<GameObject> hand)
    {
        IEnumerable<GameObject> handSort = hand.OrderBy(hand => hand.GetComponent<CardBehavior>().value);

        return handSort.ToList();
    }

    public bool IsRoyalFlush(List<GameObject> hand)
    {
        int total = 0;

        if (IsFlush(hand) && IsStraight(hand))
        {

            for (int i = 0; i < hand.Count; i++)
            {
                total = total + hand[i].GetComponent<CardBehavior>().value;
            }

            if (total == 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public bool IsFlush(List<GameObject> hand)
    { 

        Suit suit = hand[0].GetComponent<CardBehavior>().cardSuit;

        foreach(GameObject card in hand)
        {
            if(card.GetComponent<CardBehavior>().cardSuit != suit)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsStraight(List<GameObject> hand)
    {
        

        for (int i = 0; i < hand.Count() - 1; i++)
        {
            if(hand[i+1].GetComponent<CardBehavior>().cardValue - hand[i].GetComponent<CardBehavior>().cardValue != 1)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsStraightFlush(List<GameObject> hand)
    {
        if(IsFlush(hand) && IsStraight(hand))
        {
            return true;
        }

        return false;
    }

    public bool FourKind()
    {
        if(isFourKind)
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FullHouse()
    {
        if (isFullHouse)
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ThreeKind()
    {
        if (isThreeKind)
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TwoPair()
    {
        if (isTwoPair)
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool JacksHigher()
    {
        if (isJacks)
        {
            Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Reset()
    {
        isFourKind = false;
        isFullHouse = false;
        isJacks = false;
        isThreeKind = false;
        isTwoPair = false;
    }

    /// <summary>
    /// Checks the number of pairs, threes, and fours a hand has.
    /// </summary>
    /// <param name="hand">The hand to check.</param>
    public void CheckPairs(List<GameObject> hand)
    {
        var numList = new List<int>();

        foreach(GameObject card in hand)
        {
            numList.Add(card.GetComponent<CardBehavior>().cardValue);
        }

        var g = numList.GroupBy(i => i);

        int numOfPairs = 0;
        bool hasThree = false;
        bool hasFour = false;
        bool jacksHigher = false;

        foreach(var group in  g)
        {

            if(group.Count() >= 2)
            {
                numOfPairs++;
            }

            if(group.Count() == 3)
            {
                hasThree = true;
            }
            if (group.Count() == 4)
            {
                hasFour = true;
            }
            if(group.Key >= 11 && group.Count() >= 2)
            {
                jacksHigher = true;
            }

        }

        if(hasFour) // Four Kind
        {
            Debug.Log("This hand is Four of a Kind.");
            isFourKind = true;
        }
        else if(hasThree && numOfPairs >= 2 && jacksHigher) // Full House
        {
            Debug.Log("This hand is a full House.");
            isFullHouse = true;
        }
        else if(hasThree == true) // Three Kind
        {
            Debug.Log("This hand is Three of a Kind.");
            isThreeKind = true;
        }
        else if (numOfPairs >= 2) // Two Pair
        {
            Debug.Log("This hand has Two Pairs.");
            isTwoPair = true;
        }
        else if (numOfPairs == 1 && jacksHigher == true) // Jacks or Higher
        {
            Debug.Log("This hand has Jacks or Higher.");
            isJacks = true;
        }

        Debug.Log("jacks or higher: " + jacksHigher + " Number of Pairs: " + numOfPairs + " Has Three of a Kind: " + hasThree);
    }

    public int AddHandTotal(List<GameObject> hand)
    {
        int handTotal = 0;

        List<GameObject> sortedHand = SortHand(hand);

        for(int i = 0; i < sortedHand.Count; i++)
        {
            int cardVal = sortedHand[i].GetComponent<CardNonInteractable>().value;

            // Check if card is an ace and turn it into a value of 1 or 11 depending on hand total.
            if (cardVal == 11)
            {
                if(handTotal + 11 >= 22)
                {
                    cardVal = 1;
                }

                handTotal += cardVal;
            }
            else 
            {
                handTotal += cardVal;
            }
        }

        return handTotal;
    }
}
