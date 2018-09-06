using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// add function to change card images
public class Deck {

    private const int NUM_OF_CARDS = 52;
    private List<Cards> deck;
    public Sprite[] displayDeck;

    //Constructor
    public Deck()
    {
        deck = new List<Cards>();
        displayDeck = Resources.LoadAll<Sprite>("Cards/Card2");
        SetUpDeck();
    }

    //Returns the sprite of the current card.
    public Sprite GetCardImage(int currentCard)
    {
        return displayDeck[currentCard];
    }

    //Indexing operator overload.
    public Cards this[int currentCard]
    {
        get { return deck[currentCard]; }
    }
    
    //Prints the deck of cards.
    public void printDeck()
    {
        for(int i = 0; i < NUM_OF_CARDS; i++)
        {
            Debug.Log(deck[i].Value + "-" + deck[i].Suit);
        }
    }

    //Creates the deck and shuffles.
    private void SetUpDeck()
    {
        foreach (Cards.SUIT suit in Enum.GetValues(typeof(Cards.SUIT)))
        {
            foreach (Cards.VALUE value in Enum.GetValues(typeof(Cards.VALUE)))
            {
                deck.Add( new Cards { Suit = suit, Value = value });
            }
        }
        ShuffleCards();
    }

    //Randomly shuffles deck twice just in case. :)
    public void ShuffleCards()
    {
        for (int l = 0; l < 2; l++)
        {
            for (int k = 0; k < 1; k++)
            {
                for (int i = NUM_OF_CARDS - 1; i > 0; i--)
                {
                    int j = (int)UnityEngine.Random.Range(0f, i);
                    
                    //Swaps the card.
                    var temp = deck[i];
                    deck[i] = deck[j];
                    deck[j] = temp;
                    
                    //Swaps the sprite.
                    var s = displayDeck[i];
                    displayDeck[i] = displayDeck[j];
                    displayDeck[j] = s;
                }
            }
        }
    }

    //Returns the back of the deck to display on screen.
    public List<Sprite> GetCardBacks()
    {
        List<Sprite> backs = new List<Sprite>();
        backs.Add(displayDeck[52]);
        backs.Add(displayDeck[53]);
        return backs;
    }

}
