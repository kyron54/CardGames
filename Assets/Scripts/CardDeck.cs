using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardDeck : MonoBehaviour
{
    [SerializeField]
    public List<CardScriptableObject> cardDeck = new List<CardScriptableObject>();
    public List<Sprite> spriteList = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        AddCardsToDeck();

        cardDeck = ShuffleDeck(cardDeck);
    }

    /// <summary>
    /// Adds a list of 52 card scriptable objects to the deck. These objects will be pulled and used by a card to retrieve values.
    /// </summary>
    void AddCardsToDeck()
    {
        int v = 1;
        int s = 1;

        for(int i = 1; i <= 52; i++)
        {
            CardScriptableObject scriptable = CardScriptableObject.CreateInstance<CardScriptableObject>();
            scriptable.AssignValues(v, (Suit)s);
            scriptable.name = scriptable.cardName;
            scriptable.sprite = spriteList.Find(x => x.name == scriptable.cardName);

            // Changes Aces into Highest Value card
            if(scriptable.value == 1)
            {
                scriptable.value = 14;
            }

            cardDeck.Add(scriptable);
            v++;

            // Make sure to add every card from the deck, from all suits.
            if(v > 13)
            {
                v = 1;
                s++;
            }

            if(i >= 52)
            {
                Debug.Log("Deck Filled");
            }
            
        }

    }

    /// <summary>
    /// Shuffles the deck.
    /// </summary>
    /// <param name="deck">The list of cards to shuffle.</param>
    /// <returns></returns>
    public List<CardScriptableObject> ShuffleDeck(List<CardScriptableObject> deck)
    {
        System.Random random = new System.Random();
        return deck.OrderBy(x => random.Next()).ToList();
    }

    /// <summary>
    /// Reshuffles the Deck by clearing all cards from the list and adding new cards.
    /// </summary>
    public void ReshuffleDeck()
    {
        if (cardDeck.Count > 0)
        {
            cardDeck.Clear();
        }

        AddCardsToDeck();

        cardDeck = ShuffleDeck(cardDeck);
    }

    public void RemoveFirstElement()
    {
        cardDeck.RemoveAt(0);
    }
}
