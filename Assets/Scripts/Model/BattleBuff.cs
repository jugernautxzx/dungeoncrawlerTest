using System;
using UnityEngine;

public abstract class BattleBuff
{
    public int turn;
    public string id;
    public string infoId;
    public string nameInfo;

    public CharacterModel model;
    public BattleManagerLog log;

    public virtual void BuffOnTurn() { }
    public virtual void BuffOnGet() { }
    public virtual void BuffOnRemove() { }
}

public class BuffManager{

    public static BattleBuff CreateBuff(string bType, string form)
    {
        string[] forms = form.Split('|');
        BattleBuff buff;
        switch (bType)
        {
            case "AtkBuff":
                buff = new AtkBuff(forms[4]);
                break;
            case "PoisonBuff":
                buff = new PoisonBuff(forms[4]);
                break;
            default:
                new NotImplementedException("Buff formula does not exist: " + bType);
                return null;
        }
        buff.turn = int.Parse(forms[0]);
        buff.id = forms[1];
        buff.nameInfo = forms[2];
        buff.infoId = forms[3];
        return buff;
    }

}

public class AtkBuff : BattleBuff
{
    int atkIncrease;
    public float modifier;

    public AtkBuff(string mod)
    {
        modifier = float.Parse(mod);
    }

    public override void BuffOnGet()
    {
        atkIncrease = Mathf.RoundToInt(model.battleAttribute.basePAtk * modifier);
        model.battleAttribute.pAtk += atkIncrease;
    }

    public override void BuffOnRemove()
    {
        model.battleAttribute.pAtk -= atkIncrease;
    }
}

public class PoisonBuff : BattleBuff
{
    int damage;

    public PoisonBuff(string damage)
    {
        this.damage = int.Parse(damage);
    }

    public override void BuffOnGet()
    {
        log.WriteLog(model.name + " is poisoned for " + damage + " per turn.");
    }

    public override void BuffOnTurn()
    {
        model.battleAttribute.ModifyHp(-damage);
    }
}