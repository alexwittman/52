using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Big;
using Const;

public class UpgradeButton : MonoBehaviour {

    public string CostString, UpgradeString;
    public Text CostText, LevelText;
    BigInteger Cost, Upgrade;
    public int Level;
    public Chips Chip;
    public Type type;

    public enum Type
    {
        CPS,
        BET
    }

    private void Start()
    {
        Cost = new BigInteger(CostString) * Constant.PRECISION;
        Upgrade = new BigInteger(UpgradeString);

        CostText.text = "COST: " + ChipValue.Format(Cost);
        LevelText.text = "LEVEL:" + Level;
    }

    public void UpgradePurchased()
    {
        if(Chip.GetChips() >= Cost)
        {
            switch(type)
            {
                case Type.BET:
                    {
                        Chip.AddToBet(Upgrade * Constant.PRECISION);
                        print(Chip.GetBet());
                        break;
                    }
                case Type.CPS:
                    {
                        Chip.AddToCPS(Upgrade);
                        break;
                    }
            }
            Chip.AddToTotalChips(-Cost);
            Cost = Cost * (long)(1.1f * Constant.PRECISIONF) / Constant.PRECISION;
            Level++;
            CostText.text = ChipValue.Format(Cost);
            LevelText.text = "LEVEL:" + Level;
        }
    }
}
