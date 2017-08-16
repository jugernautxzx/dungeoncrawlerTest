using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleBuff
{
    public int turn;
    public string id;
    public string infoId;
    public string nameInfo;

    public CharacterModel model;
    public BattleManagerLog log;

    public List<BuffTag> tags = new List<BuffTag>();

    public void CharaTakeTurn()
    {
        BuffOnTurn();
        turn -= 1;
        if (turn <= 0)
            BuffOnRemove();
    }

    public abstract void AddTags();

    public bool IsExpired()
    {
        return turn <= 0;
    }

    public virtual void BuffOnTurn()
    {
    }
    public virtual void BuffOnGet()
    {
    }
    public virtual void BuffOnRemove()
    {
    }
}

public class BuffManager
{

    public static BattleBuff CreateBuff(string bType, string form)
    {
        string[] forms = form.Split('|');
        BattleBuff buff = GetBuffClass(bType, ref forms);
        buff.turn = int.Parse(forms[0]);
        buff.id = forms[1];
        buff.nameInfo = forms[2];
        buff.infoId = forms[3];
        return buff;
    }

    static BattleBuff GetBuffClass(string bType, ref string[] forms)
    {
        switch (bType)
        {
            case "AtkBuff":
                return new AtkBuff(forms[4]);
            case "PoisonBuff":
                return new PoisonBuff(forms[4]);
            default:
                new NotImplementedException("Buff formula does not exist: " + bType);
                return null;
        }
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
        base.BuffOnGet();
        atkIncrease = Mathf.RoundToInt(model.battleAttribute.basePAtk * modifier);
        model.battleAttribute.pAtk += atkIncrease;
    }

    public override void BuffOnRemove()
    {
        base.BuffOnRemove();
        model.battleAttribute.pAtk -= atkIncrease;
    }

    public override void AddTags()
    {
        tags.Add(BuffTag.AttribUp);
        tags.Add(BuffTag.Buff);
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
        base.BuffOnGet();
        log.WriteLog(model.name + " is poisoned for " + turn + " turn.");
    }

    public override void BuffOnTurn()
    {
        base.BuffOnTurn();
        log.WriteLog(model.name + " takes "  + damage + " poison damage.");
        model.battleAttribute.ModifyHp(-damage);
    }

    public override void BuffOnRemove()
    {
        base.BuffOnRemove();
        log.WriteLog(model.name + " is no longer poisoned.");
    }

    public override void AddTags()
    {
        tags.Add(BuffTag.Debuff);
        tags.Add(BuffTag.Poison);
    }
}