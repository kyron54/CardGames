using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hand : MonoBehaviour
{
    public GameObject card;
    public int handSize = 4;

    [SerializeField]
    public List<GameObject> hand = new List<GameObject>();

    /// <summary>
    /// Completely removes every card in a hand.
    /// </summary>
    public void ClearHand()
    {
        foreach (GameObject card in hand)
        {
            Destroy(card);
        }

        hand.RemoveRange(0, hand.Count);
    }

    /// <summary>
    /// Checks if cards are to be held before removing ones that are not.
    /// </summary>
    public void RemoveCards()
    {
        for(int i = 0; i <= handSize; i++)
        {
            if (hand[i].GetComponent<CardBehavior>().isHeld == false)
            {
                Destroy(hand[i]);
            }
        }
    }

    public void DealCards()
    {
        StartCoroutine("FillHand");
    }

    /// <summary>
    /// Pulls exactly one card into hand.
    /// </summary>
    public void PullOneCard()
    {
        GameObject newCard = Instantiate(card, transform);
        hand.Add(newCard);
    }
    
    /// <summary>
    /// Fills the hand with cards depending on size.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FillHand()
    {
        yield return new WaitForSeconds(.2f);

        if (hand.Count <= 0)
        {
            for (int i = 0; i <= handSize; i++)
            {
                GameObject newCard = Instantiate(card, transform); 
                hand.Add(newCard);

                yield return new WaitForSeconds(.2f);
            }
        }
        else
        {
            for (int i = 0; i <= handSize; i++)
            {
                if (hand[i] == null)
                {
                    GameObject newCard = Instantiate(card, transform);
                    hand[i] = newCard;
                }

                yield return new WaitForSeconds(.2f);
            }
        }
    }
}
