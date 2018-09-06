using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;
/// get rid of handvalue string function and clean it up
public class HandEvaluate {

    //Evaluate a 5 Card Poker hand

    private int HeartSum, DiamondSum, SpadeSum, ClubSum;
    private List<Cards> hand;

    //Enumeration of different hand values. 
    private enum HandValue
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }

    //Enumeration of different outcomes of comparing hands.
    public enum Winner
    {
        YouWin,
        YouLose,
        Tie
    }

    //Compares two hands to determine a winner.
    public Winner CompareHands(List<Cards> yourHand, List<Cards> enemyHand, int yourValue, int enemyValue)
    {
        //Base Case, where one hand is better than the other.
        if(yourValue != enemyValue)
        {
            if(yourValue > enemyValue)
            {
                return Winner.YouWin;
            }
            else
            {
                return Winner.YouLose;
            }
        }
        else
        {//Otherwise they have the same value, so need to check for ties.
            switch(yourValue)
            {
                case 0: //High Card Case
                    {
                        //Returns the winner based on who has the highest cards.
                        return HigherCards(yourHand, enemyHand);
                    }
                case 1: //One Pair Case
                    {
                        if(PairValue(yourHand) > PairValue(enemyHand))
                        {//If your pair is higher, you win.
                            return Winner.YouWin;
                        }
                        else if (PairValue(yourHand) < PairValue(enemyHand))
                        {//If your pair is lower, you lose.
                            return Winner.YouLose;
                        }
                        else
                        {//Otherwise the person with the highest cards wins.
                            return HigherCards(yourHand, enemyHand);
                        }
                    }
                case 2: //Two Pair Case
                    {
                        //The 4th card of a hand with two pairs will be part of the higher pair.
                        int yourHigherPair = (int)yourHand[3].Value;
                        int enemyHigherPair = (int)enemyHand[3].Value;

                        //The 2nd card of a hand with two pairs will be part of the lower pair.
                        int yourLowerPair = (int)yourHand[1].Value;
                        int enemyLowerPair = (int)enemyHand[1].Value;

                        if (yourHigherPair > enemyHigherPair)
                        {   //If your higher pair is higher than the other then you win
                            return Winner.YouWin;
                        }
                        else if (yourHigherPair < enemyHigherPair)
                        {   //If your higher pair is lower than the other then you lose
                            return Winner.YouLose;
                        }
                        else  //if the higher pairs are equal then compare lower pairs
                        {   
                            if (yourLowerPair > enemyLowerPair)
                            {   //If your lower pair is higher than the other then you win
                                return Winner.YouWin;
                            }
                            else if (yourLowerPair < enemyLowerPair)
                            {
                                //If your lower pair is lower than the other then you lose
                                return Winner.YouLose;
                            }
                            else
                            {   //if the two pairs are the same then the highest card wins
                                return HigherCards(yourHand, enemyHand);
                            }
                        }
                    }
                case 3: //Three of a Kind case
                    {
                        //The middle card of a hand with a three of a kind will be part of the three of a kind.
                        int yourThreeOfAKind = (int)yourHand[2].Value;
                        int enemyThreeOfAKind = (int)enemyHand[2].Value;

                        if (yourThreeOfAKind > enemyThreeOfAKind)
                        {//If your three of a kind is higher, you win.
                            return Winner.YouWin;
                        }
                        else if (yourThreeOfAKind < enemyThreeOfAKind)
                        {//If your three of a kind is lower, you lose.
                            return Winner.YouLose;
                        }
                        else
                        {//Otherwise the person with the highest cards wins.
                            return HigherCards(yourHand, enemyHand);
                        }
                    }
                case 4: //Straight case
                    {
                        //Whoever has the highest straight wins.
                        return HigherCards(yourHand, enemyHand);
                    }
                case 5: //Flush case
                    {   
                        //Highest card in a regular flush wins.
                        return HigherCards(yourHand, enemyHand);
                    }
                case 6: //Full House case
                    {
                        int yourHouse = (int)yourHand[2].Value;
                        int enemyHouse = (int)enemyHand[2].Value;

                        if(yourHouse > enemyHouse)
                        {//If your house is greater, you win.
                            return Winner.YouWin;
                        }
                        else
                        {//If your house is less, you lose.
                            return Winner.YouLose;
                        }   
                        //No case for a tie, because you cant have 6 of the same cards with one deck and no wild cards.
                    }
                case 7: //Four of a Kind case
                    {
                        //The middle card in a hand with a four of a kind is part of the four of a kind.
                        int yourKind = (int)yourHand[2].Value;
                        int enemyKind = (int)enemyHand[2].Value;

                        if(yourKind > enemyKind)
                        {//If your four of a kind is higher, you win.
                            return Winner.YouWin;
                        }
                        else
                        {//If your four of a kind is lower, you lose.
                            return Winner.YouLose;
                        }   
                        //There is no way to have the same 4 of a kind as the other with one deck and no wild cards.
                    }
                case 8: //Straight Flush case
                    {
                        //Whoever has the highest straight wins.
                        return HigherCards(yourHand, enemyHand);
                    }
                case 9: //Royal Flush case
                    {
                        //Result is always a tie if both hands are Royal Flushes.
                        return Winner.Tie;
                    }
                default:
                    {
                        //Only here because all paths need to return a value.
                        return Winner.Tie;
                    }
            }
        }
    }

    //Returns the winner based on who has the highest cards.
    private Winner HigherCards(List<Cards> yourHand, List<Cards> enemyHand)
    {
        //Since Cards are ordered iterate through from back to front.
        for(int i = Constant.HAND_SIZE; i > 0; i--)
        {
            if(yourHand[i - 1].Value > enemyHand[i - 1].Value)
            {//If your card is higher, you win.
                return Winner.YouWin;
            }
            else if(yourHand[i - 1].Value < enemyHand[i - 1].Value)
            {//If your card is less, you lose.
                return Winner.YouLose;
            }
        }   //In the rare case that the hands are identical then it is a tie.
        return Winner.Tie;
    }

    //Returns the value of the pair.
    private int PairValue(List<Cards> hand)
    {   //Check each possible position for a pair and return the value of the pair.
        if (hand[0].Value == hand[1].Value)
        {
            return (int)hand[0].Value;
        }
        else if (hand[1].Value == hand[2].Value)
        {
            return (int)hand[1].Value;
        }
        else if (hand[2].Value == hand[3].Value)
        {
            return (int)hand[2].Value;
        }
        else if (hand[3].Value == hand[4].Value)
        {
            return (int)hand[3].Value;
        }
        return -1; //should never get here because it is called with a hand containing a pair
    }

    //Evaluates a hand and returns the value.
    public Value EvaluateHand(List<Cards> eval, List<GameObject> cards)
    {
        Value HandVal = new Value(cards);
        hand = eval;

        //Get the sum of the suits to check for flushes.
        GetSuitSum();

        //Check highest value first because some are subsets of others.
        if (RoyalFlush())
        {
            //No need to pass 'CardsToFade' because a Royal Flush is 5 cards.
            HandVal.HandValue = (int)HandValue.RoyalFlush;
        }
        else if (StraightFlush())
        {
            //No need to pass 'CardsToFade' because a Straight Flush is 5 cards.
            HandVal.HandValue = (int)HandValue.StraightFlush;
        }
        else if (FourOfAKind(HandVal.Cards, HandVal.CardsToFade))
        {
            HandVal.HandValue = (int)HandValue.FourOfAKind;
        }
        else if (FullHouse())
        {
            //No need to pass 'CardsToFade' because a Full House is 5 cards.
            HandVal.HandValue = (int)HandValue.FullHouse;
        }
        else if (Flush())
        {
            //No need to pass 'CardsToFade' because a Flush is 5 cards.
            HandVal.HandValue = (int)HandValue.Flush;
        }
        else if (Straight())
        {
            //No need to pass 'CardsToFade' because a Straight is 5 cards.
            HandVal.HandValue = (int)HandValue.Straight;
        }
        else if (ThreeOfAKind(HandVal.Cards, HandVal.CardsToFade))
        {
            HandVal.HandValue = (int)HandValue.ThreeOfAKind;
        }
        else if (TwoPair(HandVal.Cards, HandVal.CardsToFade))
        {
            HandVal.HandValue = (int)HandValue.TwoPair;
        }
        else if (OnePair(HandVal.Cards, HandVal.CardsToFade))
        {
            HandVal.HandValue = (int)HandValue.OnePair;
        }
        else //High Card
        {
            //Fade all cards except the high one.
            HandVal.CardsToFade.Add(HandVal.Cards[0]);
            HandVal.CardsToFade.Add(HandVal.Cards[1]);
            HandVal.CardsToFade.Add(HandVal.Cards[2]);
            HandVal.CardsToFade.Add(HandVal.Cards[3]);

            HandVal.HandValue = (int)HandValue.HighCard;
        }

        HandVal.ValueText = SetHandValueText(HandVal.HandValue, hand);
        return HandVal;
    }

    //Returns a string of the hand value to be displayed.
    private string SetHandValueText(int value, List<Cards> hand)
    {
        if(value == 0)
        {//High Card
            
            //Return "Ace High", "King High" ...
            return (hand[4].Value.ToString() + " High").ToUpper();
        }
        else
        {//Everything else
            
            //Return the hand value.
            return Constant.VALUE_STRING[value].ToUpper();
        }
    }

    //Sets the sum of the suits in the hand
    private void GetSuitSum()
    {
        //Reset the sums from any previous calls.
        HeartSum = DiamondSum = SpadeSum = ClubSum = 0;

        //Sum the suits.
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].Suit == Cards.SUIT.Hearts) HeartSum++;
            else if (hand[i].Suit == Cards.SUIT.Diamonds) DiamondSum++;
            else if (hand[i].Suit == Cards.SUIT.Spades) SpadeSum++;
            else if (hand[i].Suit == Cards.SUIT.Clubs) ClubSum++;
        }
    }

    //Checks if the hand contains a royal flush.
    private bool RoyalFlush()
    {
        return Straight() && 
               Flush()    && 
               hand[hand.Count - 1].Value == Cards.VALUE.Ace;
    }

    //Checks if the hand contains a straight flush.
    private bool StraightFlush()
    {
        return Straight() && 
               Flush();
    }

    //Checks if the hand contains a four of a kind.
    private bool FourOfAKind(List<GameObject> Cards, List<GameObject> CardsToFade)
    {
        if (hand[0].Value == hand[1].Value &&
            hand[1].Value == hand[2].Value &&
            hand[2].Value == hand[3].Value)
        {//Four of a kind is the first four cards.
            
            //Fade the last card.
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if (hand[1].Value == hand[2].Value &&
                 hand[2].Value == hand[3].Value &&
                 hand[3].Value == hand[4].Value)
        {//Four of a kind is the last four cards.

            //Fade the first card.
            CardsToFade.Add(Cards[0]);
            return true;
        }

        //No four of a kind.
        return false;
    }

    //Checks if the hand contains a full house.
    private bool FullHouse()
    {
        if (hand[0].Value == hand[1].Value && hand[1].Value == hand[2].Value &&
            hand[3].Value == hand[4].Value)
        {//Three of a kind is the first three cards and pair is last two.

            return true;
        }
        else if (hand[0].Value == hand[1].Value && 
                 hand[2].Value == hand[3].Value && hand[3].Value == hand[4].Value)
        {//Pair is the first two cards and three of a kind is last three.

            return true;
        }

        //No full house.
        return false;
    }

    //Checks if the hand contains a flush.
    private bool Flush()
    {
        return (HeartSum == Constant.HAND_SIZE   ||
                DiamondSum == Constant.HAND_SIZE ||
                SpadeSum == Constant.HAND_SIZE   ||
                ClubSum == Constant.HAND_SIZE);
    }

    //Checks if the hand contains a straight.
    private bool Straight()
    {
        if (hand[0].Value + 1 == hand[1].Value &&
            hand[1].Value + 1 == hand[2].Value &&
            hand[2].Value + 1 == hand[3].Value &&
            hand[3].Value + 1 == hand[4].Value)
        {//Each card is one more than the last.
            return true;
        }

        //No straight.
        return false;
    }

    //Checks if the hand contains a three of a kind.
    private bool ThreeOfAKind(List<GameObject> Cards, List<GameObject> CardsToFade)
    {
        if (hand[0].Value == hand[1].Value && hand[1].Value == hand[2].Value)
        {//First three cards are the three of a kind.

            //Fade the last two cards.
            CardsToFade.Add(Cards[3]);
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if (hand[1].Value == hand[2].Value && hand[2].Value == hand[3].Value)
        {//Middle three cards are the three of a kind.

            //Fade first and last card.
            CardsToFade.Add(Cards[0]);
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if(hand[2].Value == hand[3].Value && hand[3].Value == hand[4].Value)
        {//Last three cards are the three of a kind.

            //Fade the first two cards.
            CardsToFade.Add(Cards[0]);
            CardsToFade.Add(Cards[1]);
            return true;
        }

        //No three of a kind.
        return false;
    }

    //Checks if the hand contains two pair.
    private bool TwoPair(List<GameObject> Cards, List<GameObject> CardsToFade)
    {
        if (hand[0].Value == hand[1].Value && hand[2].Value == hand[3].Value)
        {//First pair is first two cards, second pair is next two.

            //Fade the last card.
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if (hand[0].Value == hand[1].Value && hand[3].Value == hand[4].Value)
        {//First pair is first two, second pair is last two.

            //Fade middle card.
            CardsToFade.Add(Cards[2]);
            return true;
        }
        else if (hand[1].Value == hand[2].Value && hand[3].Value == hand[4].Value)
        {//first pair is the 2nd and 3rd cards, second pair is the last two.

            //Fade the first card.
            CardsToFade.Add(Cards[0]);
            return true;
        }

        //No two pair.
        return false;
    }

    //Checks if the hand contains a pair.
    private bool OnePair(List<GameObject> Cards, List<GameObject> CardsToFade)
    {
        if (hand[0].Value == hand[1].Value)
        {//Pair is the first two cards.

            //Fade the other cards.
            CardsToFade.Add(Cards[2]);
            CardsToFade.Add(Cards[3]);
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if (hand[1].Value == hand[2].Value)
        {//Pair is the second and third cards.

            //Fade the other cards.
            CardsToFade.Add(Cards[0]);
            CardsToFade.Add(Cards[3]);
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if (hand[2].Value == hand[3].Value)
        {//Pair is the third and fourth cards.
            
            //Fade the other cards.
            CardsToFade.Add(Cards[0]);
            CardsToFade.Add(Cards[1]);
            CardsToFade.Add(Cards[4]);
            return true;
        }
        else if (hand[3].Value == hand[4].Value)
        {//Pair is the last two cards.
            
            //Fade the other cards.
            CardsToFade.Add(Cards[0]);
            CardsToFade.Add(Cards[1]);
            CardsToFade.Add(Cards[2]);
            return true;
        }

        //No pair.
        return false;
    }

}

//Class used to return multiple things
public class Value
{
    //Values to be returned.
    public int HandValue;
    public string ValueText;
    public List<GameObject> CardsToFade;

    //List of card objects.
    public List<GameObject> Cards;

    //Constructor
    public Value(List<GameObject> objects)
    {
        CardsToFade = new List<GameObject>();
        Cards = objects;
    }
}