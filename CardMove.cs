using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Const;
/// clean up unneccessary variables and functions// otherwise shouldn't mess with this
public class CardMove : MonoBehaviour {

    Quaternion StartWidth, EndRotation;
    Vector2 EndPos, TargetSize;
    Sprite Front;
    public GameObject ClearPosition;
    public type HandType;
    bool CardFlipped;
    public Color FadeColor;

    //Hand type to determine movement routines.
    public enum type
    {
        YourHand,
        EnemyHand
    }

    void Start ()
    {
        CardFlipped = false;

        TargetSize = Constant.CARD_TARGET_SIZE;
        StartWidth = transform.rotation;
    }

    //Sets the end position and rotation of the card.
    public void SetEndPosition(GameObject pos)
    {
        EndPos = pos.transform.position;
        EndRotation = pos.transform.rotation;
        StartWidth.z = pos.transform.rotation.z;
    }

    //Sets the sprite to be changed when the card is flipped.
    public void SetFront(Sprite s)
    {
        Front = s;
    }

    //Starts the coroutines of physically dealing the card.
    public void DealCard(bool dealToYou)
    {
        //Always move and shrink card.
        StartCoroutine(MoveCard());
        StartCoroutine(ShrinkCard());

        //Flip your card and do not flip enemy card.
        if (dealToYou)
        {
            StartCoroutine(FlipCard(Constant.CARD_YOUR_TARGET_WIDTH));
        }
    }

    //Shifts the card to its new final position.
    public void ShiftCard()
    {
        //Stop all previous movement.
        StopAllCoroutines();

        //Sometimes shrinking is stopped before it is the target size.
        StartCoroutine(ShrinkCard());

        //If dealing to yourself, then flip left to right.
        if (HandType == type.YourHand)
        {
            StartCoroutine(FlipCard(Constant.CARD_ENEMY_TARGET_WIDTH));
        }

        //Starts the coroutine to move the card into final position.
        StartCoroutine(MoveCard()); 
    }

    //Moves the card off screen to be destroyed.
    public void ClearCard()
    {
        //Stop movement of the card, to start new movement.
        StopAllCoroutines();

        //Set new end position to off the screen.
        EndPos = ClearPosition.transform.position;

        //Clear the card.
        StartCoroutine(Clear());
    }

    //Moves the card from current position to end position.
    IEnumerator MoveCard()
    {
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / Constant.CARD_TIME_TO_REACH;
            transform.position = Vector2.Lerp(transform.position, EndPos, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, EndRotation, t);
            yield return null;
        }
    }

    //Shrinks the card to the target size.
    IEnumerator ShrinkCard()
    {
        float t = 0;
        while(GetComponent<RectTransform>().sizeDelta != TargetSize)
        {
            t += Time.deltaTime / Constant.CARD_TIME_TO_REACH;
            GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(GetComponent<RectTransform>().sizeDelta, TargetSize, t);
            yield return null;
        }
        GetComponent<RectTransform>().sizeDelta = TargetSize;
    }

    //Called when all enemy cards are dealt to flip them.
    public void FlipEnemyCard()
    {
        //Stop all movement coroutines
        StopAllCoroutines();

        //Continue movement and shrinking.
        StartCoroutine(MoveCard());
        StartCoroutine(ShrinkCard());

        //Flip the enemy card from top to bottom.
        StartCoroutine(FlipCard(Constant.CARD_ENEMY_TARGET_WIDTH));
    }

    //Flips the card by the way specified by TargetWidth.
    IEnumerator FlipCard(Quaternion TargetWidth)
    {
        //Makes sure not to flip a card more than once.
        if (!CardFlipped)
        {
            float t = 0;
            while (t < Constant.CARD_FLIP_TIME)
            {
                t += Time.deltaTime / Constant.CARD_TIME_TO_REACH * 2;
                transform.rotation = Quaternion.Lerp(transform.rotation, TargetWidth, t);
                yield return null;
            }
            
            //Changes image of the object to the card value image.
            GetComponent<Image>().sprite = Front;
            CardFlipped = true;

            t = 0;
            while (t < Constant.CARD_FLIP_TIME)
            {
                t += Time.deltaTime / Constant.CARD_TIME_TO_REACH * 2;
                transform.rotation = Quaternion.Lerp(transform.rotation, StartWidth, t);
                yield return null;
            }
        }
    }

    //Moves the card off screen and destroys the gameobject.
    IEnumerator Clear()
    {
        yield return StartCoroutine(MoveCard());
        Destroy(gameObject);
    }

    //Fades the card to a specified opacity.
    IEnumerator FadeCard()
    {
        yield return new WaitForSeconds(Constant.DEALT_TIME_DELAY);
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime / Constant.CARD_TIME_TO_REACH / Constant.FADE_ADD;
            GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, FadeColor, t);
            yield return null;
        }
    }
}
