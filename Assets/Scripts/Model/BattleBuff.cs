using System;
using UnityEngine;

public abstract class BattleBuff
{
    public int turn;
    public string id;
    public string name;
    public string infoId;
    public string infoName;

    public abstract void BuffOnTurn(CharacterModel model);
    public abstract void BuffOnGet(CharacterModel model);
    public abstract void BuffOnRemove(CharacterModel model);
    public virtual void Some()
    {

    }
}

public class AtkBuff : BattleBuff
{
    int atkIncrease;

    public override void BuffOnGet(CharacterModel model)
    {
        atkIncrease = Mathf.RoundToInt(model.battleAttribute.physAttack * 0.4f);
        model.battleAttribute.physAttack += atkIncrease;
    }

    public override void BuffOnRemove(CharacterModel model)
    {
        model.battleAttribute.physAttack -= atkIncrease;
    }

    public override void BuffOnTurn(CharacterModel model)
    {
    }
}

public class PoisonBuff : BattleBuff
{
    int damage;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public override void BuffOnGet(CharacterModel model)
    {
    }

    public override void BuffOnRemove(CharacterModel model)
    {
    }

    public override void BuffOnTurn(CharacterModel model)
    {
        model.battleAttribute.ModifyHp(damage);
    }
}