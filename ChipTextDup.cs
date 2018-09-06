using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Const;

public class ChipTextDup : MonoBehaviour {

    public Chips Chip;
    public Type value;

    public enum Type
    {
        TotalChips,
        CPS,
        Bet
    }

    void Update()
    {
        switch (value)
        {
            case (Type.TotalChips):
                {
                    General.Text.UpdateChipText(GetComponent<Text>(), Chip.GetChips());
                    break;
                }
            case (Type.CPS):
                {
                    GetComponent<Text>().text = ChipValue.Format(Chip.GetChipsPerSecond() * Constant.PRECISION) + " CPS";
                    break;
                }
            case (Type.Bet):
                {
                    GetComponent<Text>().text = "BET = " + ChipValue.Format(Chip.GetBet()) + " CHIPS!";
                    break;
                }
        }
    }
}
