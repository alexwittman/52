using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Big;
using Const;
/// Add GameObject Arrays for YourHand and EnemyHand to append and clear so I don't have to use GameObject.Find
public class Click : MonoBehaviour {

    HandEvaluate Evaluate;
    Deck deck;
    Hand YourHandCards, EnemyHandCards;
    List<Sprite> CardBacks;
    bool dealToYou;
    int currentCard;
    public GameObject ClickZone, Template;
    public GameObject YourText, EnemyText, textChips;
    public Chips Chip;
    public int NumberOfShuffles, Wins, Losses, Ties;
    public bool AutoClick;

    //List of GameObjects to set in the Editor for display positions.
    public List<GameObject> YourOddDisplay;
    public List<GameObject> YourEvenDisplay;
    public List<GameObject> EnemyOddDisplay;
    public List<GameObject> EnemyEvenDisplay;

    //List of GameObjects to keep track of dealt cards.
    List<GameObject> YourHandObjects;
    List<GameObject> EnemyHandObjects;
    List<GameObject> NotDealtCards;

    void Start()
    {
        //List of GameObjects to keep track of dealt and undealt cards.
        YourHandObjects = new List<GameObject>();
        EnemyHandObjects = new List<GameObject>();
        NotDealtCards = new List<GameObject>();

        YourHandCards = new Hand();
        EnemyHandCards = new Hand();
        Evaluate = new HandEvaluate();
        deck = new Deck();

        //Statistics
        Ties = Wins = Losses = NumberOfShuffles = 0;

        currentCard = Constant.DECK_FIRST_CARD;

        CardBacks = deck.GetCardBacks();

        //Start by dealing to your opponent.
        dealToYou = false;

        //Creates an initial 5 cards so there is no instance where there is no card to deal.
        for (int i = 0; i < Constant.HAND_SIZE; i++)
        {
            MakeCard();
        }

        //For testing purposes only.
        if (AutoClick) InvokeRepeating("CardManager", 1f, .1f);//for testing fast clicking
    }

    //Used to change card image when a finger is dealing a card.
    void Update()
    {
        var size = ClickZone.GetComponent<RectTransform>().sizeDelta;
        foreach (Touch touches in Input.touches)
        {
            //if (touches.position.x <= ClickZone.transform.position.x + size.x / 2 && touches.position.x >= ClickZone.transform.position.x - size.x / 2 && touches.position.y <= ClickZone.transform.position.y + size.y / 2 && touches.position.y >= ClickZone.transform.position.y - size.y / 2)
            {
                if (touches.phase == TouchPhase.Began)
                {
                    //CardsNotDealt[0].GetComponent<Image>().sprite = CardBacks[1];
                }
            }
        }
    }

    //Deals a card when the 'ClickZone' button is clicked.
    public void CardManager()
    {
        //Used when automatic testing is used to pause the game while value is shown.
        if (GetComponent<Button>().interactable == true)
        {
            //If you have 0 chips, then you cannot play.
            if (Chip.GetChips() == 0)
            {
                //Displays text that says you do not have enough chips. May add other stuff later.
                NoChips();
            }
            else
            {//Otherwise you have enough chips, so deal a card.

                //Get an undealt card.
                GameObject card = NotDealtCards[0];

                //Remove the card from the list of undealt cards.
                NotDealtCards.Remove(card);

                //If the deck has been dealt, shuffle the deck.
                if (currentCard >= Constant.DECK_SHUFFLE_TRIGGER)
                {
                    Shuffle();
                }

                //Deal the card to the appropriate player.
                VirtualDealCard();

                //Dealing the card with GameObjects.
                PhysicalDealCard(card);

                //Sets the card image of the game object.
                card.GetComponent<CardMove>().SetFront(deck.GetCardImage(currentCard));

                //Increment 'currentCard' so it doesn't deal the same card.
                currentCard++;

                //Create a new card to replace the dealt one.
                MakeCard();

                //Sets the image of the unflipped card to the card back with drop shadow.
                card.GetComponent<Image>().sprite = deck.GetCardImage(52);

                //Sets the card object to the top of the screen as to deal from the top of the deck.
                card.transform.SetAsLastSibling();

                //Sort the hands before setting final positions.
                if (dealToYou)
                {
                    YourHandCards.SortHand(YourHandObjects);
                }
                else
                {
                    EnemyHandCards.SortHand(EnemyHandObjects);
                }

                //Makes sure the next card is dealt to the opposite hand.
                dealToYou = !dealToYou;

                //This keeps the dealt cards there until the next card is dealt.
                if (EnemyHandObjects.Count == 6 && YourHandObjects.Count == 5)
                {
                    //Clears the cards, hand values, and chips from the screen.
                    ClearScreen();
                }

                //Sets the end positions of the dealt cards.
                SetPositions(YourHandObjects, YourOddDisplay, YourEvenDisplay, YourHandCards);
                SetPositions(EnemyHandObjects, EnemyOddDisplay, EnemyEvenDisplay, EnemyHandCards);

                //When both hands are full evaluate, compare and distribute chips.
                if (YourHandCards.full() && EnemyHandCards.full())
                {
                    HandsFull();
                }
            }
        }
    }

    void HandsFull()
    {
        //Makes the button not interactable for a period of time while values are displayed.
        StartCoroutine(Pause());

        //Flips all of the enemy cards at once.
        for (int i = 0; i < EnemyHandObjects.Count; i++)
        {
            EnemyHandObjects[i].GetComponent<CardMove>().FlipEnemyCard();
        }

        //Evaluates Both Hands.
        var YourValue = Evaluate.EvaluateHand(YourHandCards.GetHand(), YourHandObjects);
        var EnemyValue = Evaluate.EvaluateHand(EnemyHandCards.GetHand(), EnemyHandObjects);

        //Begins fading cards that are not a part of the value.
        FadeCards(YourValue.CardsToFade);
        FadeCards(EnemyValue.CardsToFade);

        //Determines the winner between the two hands.
        var winner = Evaluate.CompareHands(YourHandCards.GetHand(), EnemyHandCards.GetHand(), YourValue.HandValue, EnemyValue.HandValue);

        //Gets the amount of chips won to display on screen.
        BigInteger winnings = Chip.AddBet(YourValue.HandValue, winner);

        //Changes the text of the handvalue object to the current hand value.
        General.Text.HandValueText(YourText.GetComponent<Text>(), YourValue.ValueText);
        General.Text.HandValueText(EnemyText.GetComponent<Text>(), EnemyValue.ValueText);

        //Updates the text of winnings object.
        WinningsText(winner, winnings);

        //Starts the movement of the objects to display values.
        textChips.GetComponent<Slide>().StartMovement();
        YourText.GetComponent<Slide>().StartMovement();
        EnemyText.GetComponent<Slide>().StartMovement();

        //Clear both hands.
        YourHandCards.clearHand();
        EnemyHandCards.clearHand();
    }
    
    //Updates the winnings text to show how many chips are won or lost.
    void WinningsText(HandEvaluate.Winner winner, BigInteger winnings)
    {
        if (winner == HandEvaluate.Winner.YouWin)
        {//Shows green text '+Chips'
            Wins++;//stat
            textChips.GetComponent<Text>().text = "<color=#00ff00ff>+" + ChipValue.Format(winnings) + " CHIPS</color>";
        }
        else if (winner == HandEvaluate.Winner.YouLose)
        {//Shows red text '-Chips'
            Losses++;//stat
            textChips.GetComponent<Text>().text = "<color=#ff0000ff>-" + ChipValue.Format(winnings) + " CHIPS</color>";
        }
        else
        {//Shows black text 'Draw'
            Ties++;//stat
            textChips.GetComponent<Text>().text = "<color=#000000ff>DRAW</color>";
        }
    }

    //Starts the Coroutine 'FadeCard' for each card in the list.
    void FadeCards(List<GameObject> CardsToFade)
    {
        for (int i = 0; i < CardsToFade.Count; i++)
        {
            CardsToFade[i].GetComponent<CardMove>().StartCoroutine("FadeCard");
        }
    }

    //Clears the hands, values, and winnings objects from the screen.
    void ClearScreen()
    {
        //Clears the hand values from the screen.
        YourText.GetComponent<Slide>().ContinueMovement();
        EnemyText.GetComponent<Slide>().ContinueMovement();

        //Clears the winnings from the screen.
        textChips.GetComponent<Slide>().ContinueMovement();

        //Clears your cards from the screen.
        for (int i = 0; i < YourHandObjects.Count; i++)
        {
            YourHandObjects[i].GetComponent<CardMove>().ClearCard();
        }

        //Remove your cards from the list of objects.
        YourHandObjects.RemoveRange(0, 5);

        //Clears the enemy's cards from the screen.
        for (int i = 0; i < EnemyHandObjects.Count - 1; i++)
        {
            EnemyHandObjects[i].GetComponent<CardMove>().ClearCard();
        }

        //Removes the enemy's cards from the list of objects.
        EnemyHandObjects.RemoveRange(0, 5);
    }

    //Deals the current card to the virtual hand.
    void VirtualDealCard()
    {
        //Deals the card into your own hand.
        if (dealToYou)
        {
            YourHandCards.deal(deck[currentCard]);
        }
        else
        {//Deals the card into the enemy's hand.
            EnemyHandCards.deal(deck[currentCard]);
        }
    }

    //Deals the current card to the physical/visible hand.
    void PhysicalDealCard(GameObject card)
    {
        //Begins the process of physically moving the card.
        card.GetComponent<CardMove>().DealCard(dealToYou);

        //Deals the card into your own hand.
        if (dealToYou)
        {
            //Starts the Coroutine to shift all cards in your hand to the correct position.
            for (int i = 0; i < YourHandObjects.Count; i++)
            {
                YourHandObjects[i].GetComponent<CardMove>().ShiftCard();
            }

            //Adds the current card gameobject to the list of your cards.
            YourHandObjects.Add(card);

            //Sets the type of hand to your own to determine movement routines.
            card.GetComponent<CardMove>().HandType = CardMove.type.YourHand;
        }
        else
        {//Deals the card into the enemy's hand.

            //Starts the Coroutines to shift all cards in the enemy's hand to the correct position.
            for (int i = 0; i < EnemyHandObjects.Count; i++)
            {
                EnemyHandObjects[i].GetComponent<CardMove>().ShiftCard();
            }

            //Adds the current card gameobject to the list of enemy cards.
            EnemyHandObjects.Add(card);

            //Sets the type of hand to enemy to determine movement routines.
            card.GetComponent<CardMove>().HandType = CardMove.type.EnemyHand;
        }
    }

    //Shuffles the deck of cards.
    void Shuffle()
    {
        //Sets the current card to the first card in the deck.
        currentCard = Constant.DECK_FIRST_CARD;

        //Shuffles the deck.
        deck.ShuffleCards();

        NumberOfShuffles++;//stat
    }

    //Called when you do not have enough chips to play. Displays an indicator that you do not have enough.
    void NoChips()
    {
        //Makes sure not to create more than one indicator.
        if (GameObject.Find("Not Enough Chips!(Clone)") == null)
        {
            //Creates a Text gameobject
            var warning = (GameObject)Instantiate(Resources.Load("Prefabs/Not Enough Chips!"));
            warning.transform.SetParent(GameObject.Find("YourHandPositions").transform, false);

            //Fades the gameobject in and out after some time.
            StartCoroutine(General.Text.FadeTextInAndOut(warning));
        }
    }

    //Creates a new physical card on screen.
    private void MakeCard()
    {
        //Create the card.
        var temp = Instantiate(Template);
        temp.GetComponent<Transform>().SetParent(transform.parent.transform, false);

        //Insert it into the list of undealt cards.
        NotDealtCards.Insert(0, temp);
    }
    
    //Sets the end position of the cards so they move to the correct ordered position.
    private void SetPositions(List<GameObject> cards, List<GameObject> OddDisplay, List<GameObject> EvenDisplay, Hand hand)
    {
        //Sets the display to be used in setting positions of the cards.
        List<GameObject> Display = (cards.Count % 2 == 1) ? OddDisplay : EvenDisplay;

        //Different cases for the amount of cards in the hand.
        switch(cards.Count)
        {
            //Do not need 'case 0:' because no positions need to be set.
            case 1:
                {//Case: there is one card in the hand.

                    //Set the position to the middle one of OddDisplay.
                    SetCardPositions(2, cards, Display, hand);
                    break;
                }
            case 2:
                {//Case: there are two cards in the hand.

                    //Set the positions of the 2 cards to the middle two positions of the EvenDisplay.
                    SetCardPositions(1, cards, Display, hand);
                    break;
                }
            case 3:
                {//Case: there are three cards in the hand.

                    //Set the positions of the 3 cards to the middle three positions of the OddDisplay.
                    SetCardPositions(1, cards, Display, hand);
                    break;
                }
            case 4:
                {//Case: there are four cards in the hand.

                    //Set the positions of the 4 cards to the positions of the EvenDisplay.
                    SetCardPositions(0, cards, Display, hand);
                    break;
                }
            case 5:
                {//Case: there are five cards in the hand.

                    //Set the positions of the 5 cards to the positions of the OddDisplay.
                    SetCardPositions(0, cards, Display, hand);
                    break;
                }
        }
    }

    //Sets the cards' final positions so they are in order.
    void SetCardPositions(int Index, List<GameObject> cards, List<GameObject> Display, Hand hand)
    {
        //Since the SortHand function sorts the objects as well, just need to set positions, no comparison.
        for(int i = 0; i < cards.Count; i++)
        {
            cards[i].GetComponent<CardMove>().SetEndPosition(Display[Index++]);
            cards[i].transform.SetAsLastSibling();
        }
    }

    //Pauses the game so no cards can be dealt for a time while the values are displayed.
    IEnumerator Pause()
    {
        //Makes it so you cannot deal a card.
        GetComponent<Button>().interactable = false;

        //Pauses the adding of chips constantly to prevent weird text display.
        Chip.AddChipsPerSec = false;

        //Wait for time.
        yield return new WaitForSeconds(Constant.PAUSE);

        //Returns back to normal.
        GetComponent<Button>().interactable = true;
        Chip.AddChipsPerSec = true;
    }
    
}
