using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Suit
{
    Hearts = 1,
    Diamonds = 2,
    Clubs = 3,
    Spades = 4
}

public class CardBehavior : MonoBehaviour
{
    [SerializeField]
    public Suit cardSuit;
    [SerializeField]
    public int cardValue;
    protected CardScriptableObject cardObj;
    protected Image image;
    protected TextMeshProUGUI text;

    public Suit _suit {  get { return cardSuit; } }

    public int value { get { return cardValue; } }

    protected int initValue;
    protected Suit initSuit;

    protected CardDeck deck;

    public Sprite defaultSprite;

    public bool isHeld = false ;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        deck = GameObject.FindAnyObjectByType<CardDeck>();

        initValue = cardValue;
        initSuit = cardSuit;

        this.PullCard();
    }

    private void Update()
    {
        if (isHeld)
        {
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }

        //ChangeCardSprite();
    }

    /// <summary>
    /// Pulls top card from the deck and retrieves its values. Also deletes card from the deck, so it cannot be pulled until it is reshuffled.
    /// </summary>
    public void PullCard()
    {
        // Grabs the card off the top of the deck
        cardObj = deck.cardDeck[0];

        // Removes the top object from deck.
        deck.RemoveFirstElement();

        // Replaces current card with new card image
        image.sprite = cardObj.sprite;

        // Updates card values
        cardValue = cardObj.value;
        cardSuit = cardObj.suit;

        // upon running out of cards, reshuffle.
        if(deck.cardDeck.Count <= 0)
        {
            deck.ReshuffleDeck();
        }
    }

    // Test Functions

    /// <summary>
    /// Generates a random card by assigning random values. 
    /// </summary>
    public void GenerateRandomCard()
    {
        cardObj.AssignValues(Random.Range(1,14) , (Suit)Random.Range(1, 5));
        image.sprite = cardObj.sprite;

        Debug.Log(cardObj.value + " " + cardObj.suit);

        cardValue = cardObj.value;
        cardSuit = cardObj.suit;
    }

    public void LogName()
    {
        Debug.Log("Card Suit: " + _suit + " Card Value: " + cardValue);
    }

    public void BackToDefaultSprite()
    {
        image.sprite = defaultSprite;
    }

    private void ChangeCardSprite()
    {
        if (cardValue != initValue || cardSuit != initSuit)
        {
            //For testing purposes.
            string imageName = string.Format("{0}_{1}", _suit, cardValue);
            image.sprite = deck.spriteList.Find(x => x.name == imageName);
        }
    }

    /// <summary>
    /// Switches the status of whether to hold a particular card.
    /// </summary>
    public void HoldCard()
    {
        isHeld = !isHeld;
    }

}
