using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCaster : BaseMonster
{
    CharacterModel self;
    BattleManager manager;

    public BoneCaster(CharacterModel model, BattleManager manager)
    {
        self = model;
        this.manager = manager;
    }

    public void TakeTurn()
    {
        int i = Random.Range(0, 3);
        while (!manager.IsValid(manager.GetPlayer(i)))
        {
            i = Random.Range(0, 3);
        }
        manager.ActorSkillTarget(i, true, 0);
    }
}
