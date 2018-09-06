using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Big;
using Const;
/// Add some kind of indication of defeating an enemy & Chip bar
public class EnemyController : MonoBehaviour {
    Enemy CurrentEnemy;
    public GameObject Chips;
    public Slider Bar;
    public Text EnemyName, EnemyChips;
    public TextAsset FirstNameFile, LastNameFile;
    double DisplayValue;
    string[] FirstNames, LastNames;

    // Use this for initialization
    void Start ()
    {
        //Load Names from files.
        FirstNames = FirstNameFile.text.Split("\n"[0]);
        LastNames = LastNameFile.text.Split("\n"[0]);

        //Loads new enemy.
        LoadEnemy(Chips.GetComponent<Chips>().GetBet(), Chips.GetComponent<Chips>().GetChipsPerSecond());

        //Updates enemy chip text.
        EnemyChips.text = ChipValue.Format(CurrentEnemy.GetChips()) + " CHIPS";
    }

    //Creates a new enemy.
    void LoadEnemy(BigInteger Bet, BigInteger ChipsPerSecond)
    {
        //Create enemy.
        CurrentEnemy = new Enemy(Bet, ChipsPerSecond, FirstNames, LastNames);

        //Update enemy name text.
        EnemyName.text = "LVL " + CurrentEnemy.GetLevel() + " - " + CurrentEnemy.Name.ToUpper();
    }

    //Gets rid of the current enemy and loads a new one.
    public void DefeatEnemy()
    {
        //Add animations of defeating the enemy.
        LoadEnemy(Chips.GetComponent<Chips>().GetBet(), Chips.GetComponent<Chips>().GetChipsPerSecond());
    }

    //Returns the current enemy's chips.
    public BigInteger GetChips()
    {
        return CurrentEnemy.GetChips();
    }

    //Returns the current enemy's MaxChips.
    public BigInteger GetMaxChips()
    {
        return CurrentEnemy.GetMaxChips();
    }

    //Sets the chips of the current enemy.
    public void SetChips(BigInteger chips)
    {
        CurrentEnemy.SetChips(chips);

        //If the enemy has no more chips, then load a new one.
        if (chips <= 0) DefeatEnemy();
    }

    //Changes a value from current to target over xtimes iterations.
    public IEnumerator Lerp(BigInteger current, BigInteger target, int xtimes)
    {
        xtimes = (BigInteger.Abs(target - current) < xtimes) ? BigInteger.ToInt16(BigInteger.Abs(target - current)) : xtimes;
        xtimes = (xtimes == 0) ? Constant.X_TIMES : xtimes;
        BigInteger add = (target - current) / xtimes;
        for (int i = 0; i < xtimes; i++)
        {
            if (CurrentEnemy.GetChips() + add < 0)
            {
                SetChips(0);
                break;
            }
            else
            {
                SetChips(CurrentEnemy.GetChips() + add);
            }
            EnemyChips.text = ChipValue.Format(CurrentEnemy.GetChips()) + " CHIPS";
            Bar.value = (GetChips() > GetMaxChips()) ? 1 : BigInteger.Divide(GetChips(), GetMaxChips(), 0);
            yield return new WaitForSeconds(Constant.CHIP_UPDATE_TIME * Constant.X_TIMES / xtimes);
        }
    }
}
