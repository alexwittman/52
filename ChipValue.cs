using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Big;
using Const;
/// give the user options for formatting the text
public class ChipValue
{
    //Formats the large numbers to a more ledgible format.
    public static string Format(BigInteger num)
    {
        //Round up the number to account for the decimal lost when dividing.
        num = RoundUp(num);

        //Divide by precision to get rid of any decimal values.
        num /= Constant.PRECISION;

        //Convert BigInteger to string.
        string st = num.ToString();

        if (num < 1000000000)
        {//less than 1 billion format with commas
            st = Comma(st);
        }
        else
        {
            if (num < BigInteger.Pow(10, (BigInteger)39))
            {//less than 10^39 format with suffixes B, T, KT, ...
                switch ((st.Length - 1) % 3)
                {
                    case 0:
                        {// 1.234
                            st = st.Substring(0, 1) + '.' + st.Substring(1, 3) + Constant.SUFFIX[(st.Length - 1) / 3 - 3];
                            break;
                        }
                    case 1:
                        {// 12.34
                            st = st.Substring(0, 2) + '.' + st.Substring(2, 2) + Constant.SUFFIX[(st.Length - 1) / 3 - 3];
                            break;
                        }
                    case 2:
                        {// 123.4
                            st = st.Substring(0, 3) + '.' + st.Substring(3, 1) + Constant.SUFFIX[(st.Length - 1) / 3 - 3];
                            break;
                        }
                }
            }
            else
            {//bigger than that add e[number] suffix
                switch ((st.Length - 1) % 3)
                {
                    case 0:
                        {// 1.234 e3
                            st = st.Substring(0, 1) + '.' + st.Substring(1, 3) + " e" + (st.Length - 1);
                            break;
                        }
                    case 1:
                        {// 12.34 e3
                            st = st.Substring(0, 2) + '.' + st.Substring(2, 2) + " e" + (st.Length - 2);
                            break;
                        }
                    case 2:
                        {// 123.4 e3
                            st = st.Substring(0, 3) + '.' + st.Substring(3, 1) + " e" + (st.Length - 3);
                            break;
                        }
                }
            }
        }
        return st;
    }

    //Converts a string of numbers to a comma separated number.
    private static string Comma(string number)
    {// 1,000,000
        if(number.Length > 3)
        {
            for (int i = number.Length - 3; i > 0; i -= 3)
            {
                number = number.Insert(i, ",");
            }
        }
        return number;
    }

    //Adds constant to the number to make up for precision loss.
    private static BigInteger RoundUp(BigInteger num)
    {
        return num + Constant.ROUND_UP;
    }
}
