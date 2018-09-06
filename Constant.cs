using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Big;
/// Add any other constants I find
namespace Const
{
    public class Constant
    {
        public static BigInteger STARTING_CHIPS = 50;
        public static BigInteger STARTING_BET = 1;
        public static BigInteger STARTING_CHIPS_PER_SECOND = 1;

        public static BigInteger PRECISION = 1000;
        public static float PRECISIONF = 1000f;
        public static BigInteger ROUND_UP = 500;

        public static float PAUSE = 0.75f;
        public static int X_TIMES = 10;
        public static float CHIP_UPDATE_TIME = PAUSE / X_TIMES;

        public static string[] SUFFIX = { "B", "T", "KT", "MT", "BT", "TT", "KTT", "MTT", "BTT", "TTT" };
        public static string[] VALUE_STRING = { "High Card", "One Pair", "Two Pair", "Three Of A Kind", "Straight", "Flush", "Full House", "Four Of A Kind", "Straight Flush", "Royal Flush" };

        public static float[] VALUE_MULT_F = { 1.5f, 2, 20, 50, 250, 500, 1000, 5000, 100000, 1000000 };
        public static BigInteger[] VALUE_MULT = { (long)(VALUE_MULT_F[0] * PRECISIONF), (long)(VALUE_MULT_F[1] * PRECISIONF), (long)(VALUE_MULT_F[2] * PRECISIONF), (long)(VALUE_MULT_F[3] * PRECISIONF), (long)(VALUE_MULT_F[4] * PRECISIONF), (long)(VALUE_MULT_F[5] * PRECISIONF), (long)(VALUE_MULT_F[6] * PRECISIONF), (long)(VALUE_MULT_F[7] * PRECISIONF), (long)(VALUE_MULT_F[8] * PRECISIONF), (long)(VALUE_MULT_F[9] * PRECISIONF) };

        public static float FADE_ADD = 10;
        public static float FADE_TIME = 0.1f;

        public static float COLOR_CLEAR = 0;
        public static float COLOR_FULL = 1;

        public static int DECK_SIZE = 52;
        public static int DECK_SHUFFLE_TRIGGER = 50;
        public static int DECK_FIRST_CARD = 0;

        public static float CARD_TIME_TO_REACH = 0.25f;
        public static float CARD_SIZE_DIV = 2.5f;
        public static float CARD_FLIP_TIME = 0.7f;
        public static Quaternion CARD_YOUR_TARGET_WIDTH = Quaternion.Euler(0f, 90f, 0f);
        public static Quaternion CARD_ENEMY_TARGET_WIDTH = Quaternion.Euler(90f, 0f, 0f);
        public static Vector2 CARD_TARGET_SIZE = GameObject.Find("Card").GetComponent<RectTransform>().sizeDelta / CARD_SIZE_DIV;

        public static float DEALT_TIME_TO_REACH = 1f;
        public static float DEALT_TIME_DELAY = PAUSE / 2;

        public static int HAND_SIZE = 5;

        public static float POSITION_BUFFER = 1f;

        public static BigInteger ENEMY_SECONDS_TO_DEFEAT = 20;
    }
}

