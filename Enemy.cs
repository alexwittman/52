using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Big;
using Const;
/// add better chip and level generation algorithms
public class Enemy {

    BigInteger Chips, MaxChips;
    public string Name { get; set; }
    int Level;

    //Constructor
	public Enemy(BigInteger Bet, BigInteger ChipsPerSecond, string[] FirstNames, string[] LastNames)
    {
        Level = LevelGen();
        Chips = ChipGen(Bet, ChipsPerSecond);
        MaxChips = Chips;
        Name = NameGen(FirstNames, LastNames);
    }

    //Generates the enemy's chips.
    BigInteger ChipGen(BigInteger Bet, BigInteger ChipsPerSecond)
    {
        return Bet * Level + ChipsPerSecond * Constant.ENEMY_SECONDS_TO_DEFEAT * Constant.PRECISION;
    }

    //Generates the enemy's name.
    string NameGen(string[] FirstNames, string[] LastNames)
    {
        int IndexFirst = UnityEngine.Random.Range(0, FirstNames.Length);
        int IndexLast = UnityEngine.Random.Range(0, LastNames.Length);
        return FirstNames[IndexFirst] + " " + LastNames[IndexLast];
    }

    //Generates the enemy's level.
    int LevelGen()
    {
        return UnityEngine.Random.Range(1, 100);
    }

    //Returns the enemy's level.
    public int GetLevel()
    {
        return Level;
    }

    //Returns the enemy's chips.
    public BigInteger GetChips()
    {
        return Chips;
    }

    //Returns the MaxChips
    public BigInteger GetMaxChips()
    {
        return MaxChips;
    }

    //Sets the enemy's chips.
    public void SetChips(BigInteger chips)
    {
        Chips = chips;
    }
}
