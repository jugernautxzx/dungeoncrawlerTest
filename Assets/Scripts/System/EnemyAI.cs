
using UnityEngine;

public interface BaseMonster
{
    void TakeTurn();
}

public class SimpleMonster : BaseMonster
{
    CharacterModel self;
    BattleManager manager;

    public SimpleMonster(CharacterModel model, BattleManager manager)
    {
        self = model;
        this.manager = manager;
    }

    public void TakeTurn()
    {
        int i = Random.Range(0, 4);
        while (!manager.IsValid(manager.GetPlayer(i)))
        {
            i = Random.Range(0, 4);
        }
        manager.ActorAttackTarget(i, true);
    }
}

public class EnemyAI
{
    BattleManager manager;

    public void InitMonster(CharacterModel model)
    {
        switch (model.monsterId)
        {
            case MonsterId.None:
                model.monster = new SimpleMonster(model, manager);
                break;
            case MonsterId.BoneCaster:
                model.monster = new BoneCaster(model, manager);
                break;
            default:
                model.monster = new SimpleMonster(model, manager);
                break;
        }
    }

    public EnemyAI(BattleManager manager)
    {
        this.manager = manager;
    }
}
