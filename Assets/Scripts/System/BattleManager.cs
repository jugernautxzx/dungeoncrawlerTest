using System.Collections;
using UnityEngine;

public interface BattleInterface
{
    void UpdateTimer(int p1, int p2, int p3, int p4);
    void UpdatePlayerTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4);
    void UpdateEnemyTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4);
    void StartBattleTimer(IEnumerator coroutine);
    void StopBattleTimer();
    void PlayerTakeTurn(CharacterModel player);
    void EnableTargetingIndicator(int index, bool isPlayerSide, bool enabled);
    void WriteLog(string log);
    void StartEnemyCoroutine(IEnumerator coroutine);
}

public class BattleManager
{

    CharacterModel player1, player2, player3, player4, enemy1, enemy2, enemy3, enemy4;

    CharacterModel turnTaker;

    bool timer, allActorsAlive;
    BattleInterface listener;
    BattleCalculator calculate;

    public BattleManager(BattleInterface listener)
    {
        this.listener = listener;
        calculate = new BattleCalculator();
    }

    public void BattleStart()
    {
        allActorsAlive = true;
        timer = true;
        InitiateActors();
        UpdateAllTeam();
        listener.StartBattleTimer(BattleTimer());
    }

    void UpdateAllTeam()
    {
        listener.UpdatePlayerTeam(player1, player2, player3, player4);
        listener.UpdateEnemyTeam(enemy1, enemy2, enemy3, enemy4);
    }

    void InitiateActors()
    {
        //TODO Debugging purpose
        player1 = Debugger.GenerateCharacterModel("John McHammer");
        player1.isMainCharacter = true;
        player1.attribute.speed = 3;
        enemy1 = Debugger.GenerateCharacterModel("Skeleton");
        enemy1.attribute.speed = 1;
        //
        player1.GenerateBasicBattleAttribute();
        enemy1.GenerateBasicBattleAttribute();
    }

    public void RequestAttackTarget()
    {
        listener.EnableTargetingIndicator(0, false, true);
        listener.EnableTargetingIndicator(0, true, true);
    }

    public void PlayerActorAttackTarget(int index, bool isPlayerSide)
    {
        ActorAttackTarget(index, isPlayerSide);
        timer = true;
    }

    void ActorAttackTarget(int index, bool isPlayerSide)
    {
        turnTaker.battleAttribute.actionBar -= 2000;
        if (isPlayerSide)
        {
            ActorSingleTargetAttack(GetPlayer(index));
        }
        else
        {
            ActorSingleTargetAttack(GetEnemy(index));
        }
    }

    void ActorSingleTargetAttack(CharacterModel target)
    {
        int damage = calculate.DoNormalAttack(turnTaker, target);
        listener.WriteLog(turnTaker.name + " attack " + target.name + " for " + damage + " damage.");
        target.battleAttribute.currHp -= damage;
        if (target.battleAttribute.currHp < 0)
            target.battleAttribute.currHp = 0;
        UpdateAllTeam();
    }

    IEnumerator BattleTimer()
    {
        yield return new WaitForSeconds(1f);
        while (allActorsAlive)
        {
            yield return new WaitUntil(() => timer);
            yield return new WaitForSeconds(0.02f);
            AllActorsActionBarFill();
            listener.UpdateTimer(player1.battleAttribute.actionBar, enemy1.battleAttribute.actionBar, 0, 0);
            while (AnyActorCanMove())
            {
                timer = false;
                ActorTakeTurn();
                yield return new WaitUntil(() => timer);
            }
        }
        listener.StopBattleTimer();
    }

    void AllActorsActionBarFill()
    {
        ActorActionBarFill(player1);
        ActorActionBarFill(enemy1);
    }

    void ActorActionBarFill(CharacterModel actor)//TODO Calculation needed
    {
        if (actor == null)
            return;
        if (actor.battleAttribute.hp <= 0)
            return;
        actor.battleAttribute.actionBar += actor.battleAttribute.speed + 20;
    }

    bool AnyActorCanMove()
    {
        for(int i=0; i<4; i++)
        {
            if (IsActorCanMove(GetPlayer(i)) || IsActorCanMove(GetEnemy(i)))
                return true;
        }
        return false;
    }

    bool IsActorCanMove(CharacterModel actor)
    {
        if (actor == null)
            return false;
        return actor.battleAttribute.actionBar >= 2000;
    }

    void ActorTakeTurn()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerActorTakeTurn(GetPlayer(i)))
                return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (EnemyActorTakeTurn(GetEnemy(i)))
                return;
        }
    }

    bool PlayerActorTakeTurn(CharacterModel player)
    {
        if (player == null)
            return false;
        if (player.battleAttribute.actionBar >= 2000)
        {
            turnTaker = player;
            listener.PlayerTakeTurn(player);
            return true;
        }
        else
            return false;
    }

    bool EnemyActorTakeTurn(CharacterModel enemy)
    {
        if (enemy == null)
            return false;
        if (enemy.battleAttribute.actionBar >= 2000)
        {
            turnTaker = enemy;
            listener.StartEnemyCoroutine(AITakeTurn());
            return true;
        }
        else
            return false;
    }

    IEnumerator AITakeTurn()
    {
        yield return new WaitForSeconds(0.5f);
        ActorAttackTarget(0, true);
        yield return new WaitForSeconds(0.25f);
        timer = true;
    }

    CharacterModel GetPlayer(int index)
    {
        switch (index)
        {
            case 0:
                return player1;
            case 1:
                return player2;
            case 2:
                return player3;
            case 3:
                return player4;
            default:
                return null;
        }
    }

    CharacterModel GetEnemy(int index)
    {
        switch (index)
        {
            case 0:
                return enemy1;
            case 1:
                return enemy2;
            case 2:
                return enemy3;
            case 3:
                return enemy4;
            default:
                return null;
        }
    }
}
