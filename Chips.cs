using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Big;
using Const;
/// clean this up, make it look nice
public class Chips : MonoBehaviour {

    BigInteger TotalChips, ChipsPerSec, Bet;
    public bool AddChipsPerSec;
    public GameObject Enemy;

    private void Awake()
    {
        //Need to load saved values once I save them. Should make a function.
        TotalChips = Constant.STARTING_CHIPS * Constant.PRECISION;
        ChipsPerSec = Constant.STARTING_CHIPS_PER_SECOND;
        Bet = Constant.STARTING_BET * Constant.PRECISION;
    }

    void Start ()
    {
        //Start adding chips per second on start.
        AddChipsPerSec = true;

        //Initialize the text to display current chips.
        GetComponent<Text>().text = ChipValue.Format(TotalChips) + " CHIPS!";

        //Call 'UpdateChips' function every 'Constant.CHIP_UPDATE_TIME' seconds.
        //Better than 'Update' especially when converting a very large BigInteger to string.
        InvokeRepeating("UpdateChips", 0, Constant.CHIP_UPDATE_TIME);
    }

    //Adds chips "continuously" to 'TotalChips' and takes them away from the enemy chips.
    void UpdateChips ()
    {
        //Only adds chips while 'AddChipsPerSec' is true.
        if (AddChipsPerSec)
        {
            //Add chips to 'TotalChips'.
            TotalChips += ChipsPerSec * (long)(Constant.CHIP_UPDATE_TIME * Constant.PRECISIONF);

            //Take chips from the enemy.
            var enemy = Enemy.GetComponent<EnemyController>();
            enemy.SetChips(enemy.GetChips() - ChipsPerSec * (long)(Constant.CHIP_UPDATE_TIME * Constant.PRECISIONF));

            //Updates the slider bar value to show how many chips the enemy has compared to the max.
            enemy.Bar.value = (enemy.GetChips() > enemy.GetMaxChips()) ? 1 : BigInteger.Divide(enemy.GetChips(), enemy.GetMaxChips(), 0);

            //Update chip text for both players.
            General.Text.UpdateChipText(GetComponent<Text>(), TotalChips);
            General.Text.UpdateChipText(enemy.EnemyChips, enemy.GetChips());
        }
    }

    //Adds or subtracts chips based on the winner.
    public BigInteger AddBet(int HandValue, HandEvaluate.Winner winner)
    {
        //If you win, then add your bet multiplied by the value multiplier.
        if (winner == HandEvaluate.Winner.YouWin)
        {
            //You cannot bet more chips than you have.
            BigInteger TempBet = (Bet > TotalChips) ? TotalChips : Bet;

            AddChips(TempBet, Constant.VALUE_MULT[HandValue]);

            //Returns the value of your winnings to display on screen.
            return TempBet * Constant.VALUE_MULT[HandValue] / Constant.PRECISION;
        }//If you lose, then you only lose your bet.
        else if (winner == HandEvaluate.Winner.YouLose)
        {
            //You cannot lose more chips than you have.
            BigInteger TempBet = (Bet > TotalChips) ? TotalChips : Bet;

            AddChips(TempBet, -1 * Constant.PRECISION);
            
            //Cannot have chips go below zero, so return either 'Bet' or 'TotalChips' to display on screen.
            if (TotalChips - Bet <= 0)
            {
                return TotalChips;
            }
            else
            {
                return Bet;
            }
        }//In the extremely rare case that you tie, nobody loses anything.
        else
        {
            return 0;
        }
    }

    //Starts coroutines to add chips over time.
    void AddChips(BigInteger chips, BigInteger mult)
    {
        //Gets rid of any decimal components.
        chips /= Constant.PRECISION;

        //Starts the update routines to change the chips overtime.
        StartCoroutine(Lerp(TotalChips, TotalChips + chips * mult, Constant.X_TIMES));
        var enemy = Enemy.GetComponent<EnemyController>();
        enemy.StartCoroutine(enemy.Lerp(enemy.GetChips(), enemy.GetChips() - chips * mult, Constant.X_TIMES));
    }

    //Gets the value of the hand as a string.
    public string HandValueText(int HandValue)
    {
        return Constant.VALUE_STRING[HandValue];
    }

    //Gets the Bet value.
    public BigInteger GetBet()
    {
        return Bet;
    }

    //Adds value to Bet.
    public void AddToBet(BigInteger add)
    {
        Bet += add;
    }

    //Returns TotalChips.
    public BigInteger GetChips()
    {
        return TotalChips;
    }

    //Adds value to TotalChips.
    public void AddToTotalChips(BigInteger add)
    {
        TotalChips += add;
    }

    //Returns ChipsPerSec.
    public BigInteger GetChipsPerSecond()
    {
        return ChipsPerSec;
    }

    //Adds value to ChipsPerSecond.
    public void AddToCPS(BigInteger add)
    {
        ChipsPerSec += add;
    }

    //Temporary function for button on top of screen.
    public void DoubleBet()
    {
        Bet *= 10;
        ChipsPerSec *= 10;
    }

    //Changes 'TotalChips' to 'target' over time.
    IEnumerator Lerp(BigInteger current, BigInteger target, int xtimes)
    {
        //Cannot have chips go below zero.
        target = (target < 0) ? 0 : target;

        //If the difference is less than 'xtimes' then only update that many times.
        xtimes = (BigInteger.Abs(target - current) < xtimes) ? BigInteger.ToInt16(BigInteger.Abs(target - current)) : xtimes;

        //If current == target, then xtimes = 0, which gives a divide by zero error.
        xtimes = (xtimes == 0) ? Constant.X_TIMES : xtimes;

        //Sets 'add' to the fraction to be added over time.
        BigInteger add = (target - current) / xtimes;

        //Adds 'add' to 'TotalChips' 'xtimes' times.
        for (int i = 0; i < xtimes; i++)
        {
            if (TotalChips + add < 0)
            {
                TotalChips = 0;
            }
            else
            {
                TotalChips += add;
            }

            //Update chip text.
            General.Text.UpdateChipText(GetComponent<Text>(), TotalChips);

            //Wait a little time before adding more.
            yield return new WaitForSeconds(Constant.CHIP_UPDATE_TIME * Constant.X_TIMES / xtimes);
        }
    }
}
