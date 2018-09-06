using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class Hand
{

    private List<Cards> hand;

    public Hand()
    {
        hand = new List<Cards>();
    }

    //Returns the array of cards.
    public List<Cards> GetHand()
    {
        return hand;
    }

    //Deals a card into the hand.
    public void deal(Cards card)
    {
        hand.Add(card);
    }

    //Prints the hand.
    public void printHand()
    {
        Debug.Log("Hand:");
        for (int i = 0; i < hand.Count; i++)
        {
            Debug.Log(hand[i].Value + "-" + hand[i].Suit);
        }
    }

    //Returns true if the hand is full.
    public bool full()
    {
        return hand.Count == Constant.HAND_SIZE;
    }

    //Clears all the cards from the hand.
    public void clearHand()
    {
        hand.Clear();
    }

    //Bubble sort to sort the cards and gameobjects.
    public void SortHand(List<GameObject> VisualHand)
    {
        int comparisons = 0;
        bool swaps = true;
        while (swaps)
        {
            swaps = false;
            for (int i = 0; i < hand.Count - 1; i++)
            {
                comparisons++;
                if (hand[i] > hand[i + 1])
                {
                    //Swaps cards.
                    var temp = hand[i];
                    hand[i] = hand[i + 1];
                    hand[i + 1] = temp;

                    //Swaps objects.
                    var tempobj = VisualHand[i];
                    VisualHand[i] = VisualHand[i + 1];
                    VisualHand[i + 1] = tempobj;

                    swaps = true;
                }
            }
        }
        //Debug.Log("Comparisons = " + comparisons);
    }
}
