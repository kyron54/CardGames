using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerHand : MonoBehaviour
{
    public GameObject card;
    public int handSize = 4;

    [SerializeField]
    public List<GameObject> hand = new List<GameObject>();

    /// <summary>
    /// Clears the hand of all cards.
    /// </summary>
    public void ClearHand()
    {
        foreach(GameObject card in hand)
        {
            Destroy(card);
        }
        hand.RemoveRange(0, hand.Count);
    }

    /// <summary>
    /// Pulls exactlty one card to the hand.
    /// </summary>
    public void PullOneCard()
    {
        GameObject newCard = Instantiate(card, transform);
        hand.Add(newCard);
    }
}
