using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "ScriptableObjects/Cards")]

public class CardScriptableObject : ScriptableObject
{
    public string cardName;
    public int value;
    public List<Sprite> spriteList = new List<Sprite>();
    public Sprite sprite;

    public Suit suit;

    /// <summary>
    /// Assigns the values of the card, including its status, suit, and name.
    /// </summary>
    /// <param name="value_">The status of the card in relation to others. (Ace is '1' while King is '13')</param>
    /// <param name="suit_">The suit of the card. (Hearts, Spades, ect...) </param>
    public void AssignValues(int value_, Suit suit_)
    {
        value = value_;
        suit = suit_;

        // Formats a name that can be used to grab the appropriate image.
        cardName = string.Format("{0}_{1}", suit_, value_);
    }

}
