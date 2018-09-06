using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class Slide : MonoBehaviour
{

    private Vector2 ShowPos, EndPos, StartPos;
    public Direction STARTING_SIDE;

    //Indicates direction of movement. Can be set in the Unity Editor.
    public enum Direction
    {
        LEFT,
        RIGHT
    }

    //Starts object on opposite side, off the screen, and moves in the direction indicated.
    void Start()
    {
        Vector2 OFF_SCREEN_LEFT = new Vector2(-Screen.width / 2, transform.position.y);
        Vector2 OFF_SCREEN_RIGHT = new Vector2(3 * Screen.width / 2, transform.position.y);
        if (STARTING_SIDE == Direction.RIGHT)
        {
            StartPos = OFF_SCREEN_RIGHT;
            EndPos = OFF_SCREEN_LEFT;
        }
        else
        {
            StartPos = OFF_SCREEN_LEFT;
            EndPos = OFF_SCREEN_RIGHT;
        }

        //Sets 'Show' to a position in the middle of the screen.
        ShowPos = new Vector2(Screen.width / 2, transform.position.y);

        transform.position = StartPos;
    }

    //Starts the movement of the object.
    public void StartMovement()
    {
        //Stop all previously called Coroutines to prevent weird movement.
        StopAllCoroutines();
        transform.position = StartPos;

        //Starts movement to the 'Show' position with time delay.
        StartCoroutine(MoveObject(ShowPos, true));
    }

    //Continues the movement of the object to the 'EndPos'
    public void ContinueMovement()
    {
        //Stop all previously called Coroutines to prevent weird movement.
        StopAllCoroutines();

        //Starts Movement of object to the 'EndPos' without time delay.
        StartCoroutine(MoveObject(EndPos, false));
    }

    //Moves an object to 'Pos' over 'Constant.DEALT_TIME_TO_REACH / 2' seconds after a delay.
    IEnumerator MoveObject(Vector2 Pos, bool Delay)
    {
        if(Delay) yield return new WaitForSeconds(Constant.DEALT_TIME_DELAY);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / Constant.PAUSE / 2;
            transform.position = Vector2.Lerp(transform.position, Pos, t);
            yield return null;
        }
    }

}
