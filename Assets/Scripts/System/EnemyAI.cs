using System;

public interface BaseMonster
{
    void TakeTurn();
}

public class SimpleMonster : BaseMonster
{
    CharacterModel self;

    public SimpleMonster(CharacterModel model)
    {
        self = model;
    }

    public void TakeTurn()
    {
        
    }
}

public class EnemyAI
{
    BattleManager manager;

    public void InitMonster(CharacterModel model)
    {
        model.monster = new SimpleMonster(model);
    }

    public EnemyAI(BattleManager manager)
    {
        this.manager = manager;
    }

    public void TakeTurn()
    {

    }

}
