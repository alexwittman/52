using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Big;
using Const;
using UnityEngine.UI;
using System;

//Namespace for general functions.
namespace General
{
    //Math functions.
    public class Math
    {
        
    }

    //Text manipulation functions.
    public class Text : MonoBehaviour
    {
        //Updates the text to display the chips.
        public static void UpdateChipText(UnityEngine.UI.Text UIText, BigInteger chips)
        {
            UIText.text = ChipValue.Format(chips) + " CHIPS";
        }

        //Updates the text to display the handvalue.
        public static void HandValueText(UnityEngine.UI.Text UIText, string handValue)
        {
            UIText.text = handValue;
        }

        //Fades the text object in and out.
        public static IEnumerator FadeTextInAndOut(GameObject Object)
        {
            var text = Object.GetComponent<UnityEngine.UI.Text>();
            Color FadeColor = text.color;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            float t = 0;
            while (t < Constant.FADE_TIME)
            {
                t += Time.deltaTime / Constant.DEALT_TIME_TO_REACH / Constant.FADE_ADD;
                text.color = Color.Lerp(text.color, FadeColor, t);
                yield return null;
            }
            FadeColor.a = Constant.COLOR_CLEAR;
            t = 0;
            while (t < Constant.FADE_TIME)
            {
                t += Time.deltaTime / Constant.DEALT_TIME_TO_REACH / Constant.FADE_ADD;
                text.color = Color.Lerp(text.color, FadeColor, t);
                yield return null;
            }
            Destroy(Object.gameObject);
        }
    }

    public class Util
    {
        //General swap function.
        public static void Swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }

    }
}

