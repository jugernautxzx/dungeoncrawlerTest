using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCaster : SimpleMonster
{
    public BoneCaster(CharacterModel model, BattleManager manager) : base(model, manager)
    {
    }

    public override void TakeTurn()
    {
        int i = Random.Range(0, 4);
        while (!manager.IsValid(manager.GetPlayer(i)))
        {
            i = Random.Range(0, 4);
        }
        manager.ActorSkillTarget(i, true, 0);
    }
}