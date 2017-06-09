using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCalculator {

    public int DoNormalAttack(CharacterModel attack, CharacterModel target)
    {//TODO Do a real calculation here
        int simple = attack.battleAttribute.physAttack - target.battleAttribute.physDef;
        simple = simple <= 0 ? 1 : simple;
        return simple;
    }

    public int DoSkillDamageCalc(CharacterModel user, CharacterModel target, ActiveEffect effect)
    {//TODO do a real calculation here
        switch (effect.formula)
        {
            case "PAtk":
                return FormulaPAtk(GetValue(effect.formulaParam), user.battleAttribute.physAttack);
            case "MAtk":
                return FormulaMAtk(GetValue(effect.formulaParam), user.battleAttribute.physAttack);
            default:
                Debug.Log("Formula ID " + effect.formula + " not found.");
                return 0;
        }
    }


    int FormulaPAtk(float value, int PAtk)
    {
        return Mathf.RoundToInt(value * PAtk);
    }

    int FormulaMAtk(float value, int MAtk)
    {
        return Mathf.RoundToInt(value * MAtk);
    }

    // DEPRECATED, leave it
    private void DoOperation(ref float res, string[] split, int index)
    {
        float flo = GetValue(split[index + 1]);
        switch (split[index])
        {
            case "+":
                res += flo;
                break;
            case "-":
                res -= flo;
                break;
            case "*":
                res *= flo;
                break;
            case "/":
                res /= flo;
                break;
        }
        if (index < (split.Length - 2))
        {
            DoOperation(ref res, split, index + 2);
        }
    }

    private float GetValue(string val)
    {
        return float.Parse(val);
    }
}
