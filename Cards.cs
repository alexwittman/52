using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards
{
    //The four different suits.
    public enum SUIT
    {
        Hearts,
        Diamonds,
        Spades,
        Clubs
    }

    //Different values of the cards.
    public enum VALUE
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    //Functions to get suit and value of the card.
    public SUIT Suit { get; set; }
    public VALUE Value { get; set; }

    //Less than operator overload.
    public static bool operator <(Cards x, Cards y)
    {
        return (x.Value < y.Value || (x.Value == y.Value && x.Suit < y.Suit)) ? true : false;
    }

    //Greater than operator overload.
    public static bool operator >(Cards x, Cards y)
    {
        return (x.Value > y.Value || (x.Value == y.Value && x.Suit > y.Suit)) ? true : false;
    }
}
