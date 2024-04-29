using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardNonInteractable : CardBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        deck = GameObject.FindAnyObjectByType<CardDeck>();

        this.PullCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeCardValues()
    {
        if(cardObj.value == 11 || cardObj.value == 12 || cardObj.value == 13)
        {
            cardValue = 10;
        } 
        else if(cardObj.value == 14)
        {
            cardValue = 11;
        }
        else
        {
            cardValue = cardObj.value;
            cardSuit = cardObj.suit;
        }
    }

    /// <summary>
    /// Pulls top card from the deck and retrieves its values. Also deletes card from the deck, so it cannot be pulled until it is reshuffled.
    /// </summary>
    public new void PullCard()
    {
        cardObj = deck.cardDeck[0];

        // Removes the top object from deck.
        deck.RemoveFirstElement();

        // Replaces current card with new card image
        image.sprite = cardObj.sprite;

        // Updates card values
        ChangeCardValues();

        // upon running out of cards, reshuffle.
        if (deck.cardDeck.Count <= 0)
        {
            deck.ReshuffleDeck();
        }
    }

    public void SetValue(int value_)
    {
        cardValue = value_;
    }
}
