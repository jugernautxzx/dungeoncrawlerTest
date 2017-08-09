using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface BattleInterface
{
    void UpdateTimer(int p1, int p2, int p3, int p4);
    void UpdatePlayerTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4);
    void UpdateEnemyTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4);
    void StartBattleTimer(IEnumerator coroutine);
    void StopBattleTimer();
    void PlayerTakeTurn(CharacterModel player);
    void EnableTargetingIndicator(int index, bool isPlayerSide, bool enabled);
    void WriteLog(string log, bool clear);
    void StartEnemyCoroutine(IEnumerator coroutine);
}

public interface BattleManagerLog
{
    void WriteLog(string log);
}

public class BattleManager : BattleManagerLog
{

    CharacterModel player1, player2, player3, player4, enemy1, enemy2, enemy3, enemy4;

    CharacterModel turnTaker;
    int turnTakerIndex;

    bool timer;
    BattleInterface listener;
    BattleCalculator calculate;
    ActiveSkillManager activeManager;
    PassiveManager passiveSkill;
    ActiveUse active;
    EnemyAI enemyAI;

    public BattleManager(BattleInterface listener)
    {
        this.listener = listener;
        calculate = new BattleCalculator();
        activeManager = ActiveSkillManager.GetInstance();
        passiveSkill = new PassiveManager();
        active = new ActiveUse(this);
        enemyAI = new EnemyAI(this);
    }

    public void BattleStart()
    {
        timer = true;
        InitiateActors();
        UpdateAllTeam();
        listener.StartBattleTimer(BattleTimer());
    }

    public void UpdateAllTeam()
    {
        listener.UpdatePlayerTeam(player1, player2, player3, player4);
        listener.UpdateEnemyTeam(enemy1, enemy2, enemy3, enemy4);
    }

    void InitiateActors()
    {
        //TODO Debugging purpose
        player1 = PlayerSession.GetProfile().characters[0];
        player1.GenerateBasicBattleAttribute();
        if (PlayerSession.GetProfile().party.member1 > 0)
        {
            player2 = PlayerSession.GetProfile().characters[PlayerSession.GetProfile().party.member1];
        }
        if (PlayerSession.GetProfile().party.member2 > 0)
        {
            player3 = PlayerSession.GetProfile().characters[PlayerSession.GetProfile().party.member2];
        }
        if (PlayerSession.GetProfile().party.member3 > 0)
        {
            player4 = PlayerSession.GetProfile().characters[PlayerSession.GetProfile().party.member3];
        }

        enemy1 = MonsterLoader.LoadMonsterData(DungeonModel.enemy1, DungeonModel.lvEnemy1);
        //enemy2 = MonsterLoader.LoadMonsterData(DungeonModel.enemy2, DungeonModel.lvEnemy2);
        //enemy3 = MonsterLoader.LoadMonsterData(DungeonModel.enemy3, DungeonModel.lvEnemy3);
        //enemy4 = MonsterLoader.LoadMonsterData(DungeonModel.enemy4, DungeonModel.lvEnemy4);
        InitEnemy(enemy1);
        //InitEnemy(enemy2);
        //InitEnemy(enemy3);
        //InitEnemy(enemy4);
    }

    void InitEnemy(CharacterModel model)
    {
        if (model != null)
        {
            model.GenerateBasicBattleAttribute();
            enemyAI.InitMonster(model);
        }
    }

    public void WriteLog(string log)
    {
        listener.WriteLog(log, false);
    }

    public void RequestAttackTarget()
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsValid(GetPlayer(i)))
                listener.EnableTargetingIndicator(i, true, true);
            if (IsValid(GetEnemy(i)))
                listener.EnableTargetingIndicator(i, false, true);
        }
    }

    public string GetSkillNote(int index)
    {
        return activeManager.GetActive(turnTaker.actives[index]).info;
    }

    public void RequestSkillTarget(int skillIndex)
    {
        SetSkillTarget(activeManager.GetActive(turnTaker.actives[skillIndex]));
    }

    void SetSkillTarget(ActiveSkill active)
    {
        switch (active.target)
        {
            case Targetable.self:
                listener.EnableTargetingIndicator(turnTakerIndex, true, true);
                break;
            case Targetable.singleenemy:
                SetAllEnemy(active.row);
                break;
            case Targetable.singleparty:
                SetAllParty(active.row);
                break;
            case Targetable.allenemy:
                SetAllEnemy(active.row);
                break;
            case Targetable.allparty:
                SetAllParty(active.row);
                break;
            case Targetable.everyone:
                SetAllParty(active.row);
                SetAllEnemy(active.row);
                break;
            default:
                break;
        }
    }

    void SetAllParty(Row row)
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsValid(GetPlayer(i)))
                if (row == Row.all)
                    listener.EnableTargetingIndicator(i, true, true);
                else if (row == Row.front)
                {
                    if (!GetPlayer(i).battleAttribute.backRow)
                        listener.EnableTargetingIndicator(i, true, true);
                }
                else
                {
                    if (GetPlayer(i).battleAttribute.backRow)
                        listener.EnableTargetingIndicator(i, true, true);
                }
        }
    }

    void SetAllEnemy(Row row)
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsValid(GetEnemy(i)))
                if (row == Row.all)
                    listener.EnableTargetingIndicator(i, false, true);
                else if (row == Row.front)
                {
                    if (!GetEnemy(i).battleAttribute.backRow)
                        listener.EnableTargetingIndicator(i, false, true);
                }
                else
                {
                    if (GetEnemy(i).battleAttribute.backRow)
                        listener.EnableTargetingIndicator(i, false, true);
                }
        }
    }

    //------------------------------- NORMAL ATTACK ---------------------------------------------------------------------------------------------------

    public void PlayerActorAttackTarget(int index, bool isPlayerSide)
    {
        if (IsValid(GetCharacter(index, isPlayerSide)))
        {
            listener.WriteLog("", true);
            ActorAttackTarget(index, isPlayerSide);
            timer = true;
        }
        else
            listener.WriteLog("Please select a valid target.", true);
    }

    public void ActorAttackTarget(int index, bool isPlayerSide)
    {
        turnTaker.battleAttribute.actionBar -= 2000;
        ActorSingleTargetAttack(GetCharacter(index, isPlayerSide));
    }

    void ActorSingleTargetAttack(CharacterModel target)
    {
        int damage = calculate.DoNormalAttack(turnTaker, target);
        listener.WriteLog(turnTaker.name + " attack " + target.name + " for " + damage + " damage.", false);
        target.battleAttribute.ModifyHp(-damage);
        UpdateAllTeam();
        WriteActorStillAlive(target);
    }
    //------------------------------- END OF NORMAL ATTACK ---------------------------------------------------------------------------------------------------
    //------------------------------- ACTIVE SKILLS ----------------------------------------------------------------------------------------------------------

    public void PlayerActorSkillTarget(int index, bool isPlayerSide, int skillIndex)
    {
        listener.WriteLog("", true);
        ActorSkillTarget(index, isPlayerSide, skillIndex);
        timer = true;
    }

    public void ActorSkillTarget(int index, bool isPlayerSide, int skillIndex)
    {
        listener.WriteLog(turnTaker.name + " use " + activeManager.GetName(turnTaker.actives[skillIndex]), false);
        active.SetTurn(turnTaker);
        active.PlayerActorSkillTarget(index, isPlayerSide, skillIndex);
        WriteActorStillAlive(GetCharacter(index, isPlayerSide));
    }

    //------------------------------- END OF ACTIVE SKILLS ----------------------------------------------------------------------------------------------------------

    IEnumerator BattleTimer()
    {
        listener.WriteLog("Battle start", true);
        yield return new WaitForSeconds(1f);
        while (CheckAllActorStillAlive())
        {
            yield return new WaitUntil(() => timer);
            yield return new WaitForFixedUpdate();
            AllActorsActionBarFill();
            listener.UpdateTimer(GetTimer(player1), GetTimer(player2), GetTimer(player3), GetTimer(player4));
            while (AnyActorCanMove() && CheckAllActorStillAlive())
            {
                timer = false;
                ActorTakeTurn();
                yield return new WaitUntil(() => timer);
                listener.UpdateTimer(GetTimer(player1), GetTimer(player2), GetTimer(player3), GetTimer(player4));
            }
        }
        OnBattleFinished();
        yield return new WaitForSeconds(3);
        DungeonModel.battleWon = true;
        UnloadScene();
        listener.StopBattleTimer();
    }

    int GetTimer(CharacterModel model)
    {
        if (model == null)
            return 0;
        else
            return model.battleAttribute.actionBar;
    }

    void AllActorsActionBarFill()
    {
        ActorActionBarFill(player1);
        ActorActionBarFill(player2);
        ActorActionBarFill(player3);
        ActorActionBarFill(player4);
        ActorActionBarFill(enemy1);
        ActorActionBarFill(enemy2);
        ActorActionBarFill(enemy3);
        ActorActionBarFill(enemy4);
    }

    void ActorActionBarFill(CharacterModel actor)//TODO Calculation needed
    {
        if (!IsValid(actor))
            return;
        actor.battleAttribute.actionBar += actor.battleAttribute.speed + 20;
    }

    bool AnyActorCanMove()
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsActorCanMove(GetPlayer(i)) || IsActorCanMove(GetEnemy(i)))
                return true;
        }
        return false;
    }

    bool IsActorCanMove(CharacterModel actor)
    {
        if (!IsValid(actor))
            return false;
        return actor.battleAttribute.actionBar >= 2000;
    }

    void ActorTakeTurn()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerActorTakeTurn(GetPlayer(i)))
            {
                turnTakerIndex = i;
                return;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (EnemyActorTakeTurn(GetEnemy(i)))
                return;
        }
    }

    bool PlayerActorTakeTurn(CharacterModel player)
    {
        if (!IsValid(player))
            return false;
        if (player.battleAttribute.actionBar >= 2000)
        {
            turnTaker = player;
            TurnTakerBuffOnTurn();
            if (IsValid(turnTaker))
            {
                listener.PlayerTakeTurn(player);
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    bool EnemyActorTakeTurn(CharacterModel enemy)
    {
        if (!IsValid(enemy))
            return false;
        if (enemy.battleAttribute.actionBar >= 2000)
        {
            turnTaker = enemy;
            TurnTakerBuffOnTurn();
            if (IsValid(turnTaker))
            {
                listener.StartEnemyCoroutine(AITakeTurn());
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    void TurnTakerBuffOnTurn()
    {
        List<BattleBuff> toRemove = new List<BattleBuff>();
        foreach (BattleBuff buff in turnTaker.battleAttribute.buffs)
        {
            buff.CharaTakeTurn();
            if (buff.IsExpired())
                toRemove.Add(buff);
        }
        foreach (BattleBuff buff in toRemove)
        {
            turnTaker.battleAttribute.buffs.Remove(buff);
        }
        WriteActorStillAlive(turnTaker);
    }

    public bool IsValid(CharacterModel model)
    {
        if (model == null)
            return false;
        if (model.battleAttribute.currHp <= 0)
            return false;
        return true;
    }

    IEnumerator AITakeTurn()
    {
        Debug.Log("Waiting for " + turnTaker.name);
        yield return new WaitForSeconds(0.5f);
        //TODO Insert EnemyAI
        //ActorAttackTarget(0, true);
        turnTaker.monster.TakeTurn();
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Done waiting for " + turnTaker.name);
        timer = true;
    }

    public CharacterModel GetCharacter(int index, bool isPlayerSide)
    {
        if (isPlayerSide)
            return GetPlayer(index);
        else
            return GetEnemy(index);
    }

    public CharacterModel GetPlayer(int index)
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

    public CharacterModel GetEnemy(int index)
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

    void WriteActorStillAlive(CharacterModel model)
    {
        if (!IsValid(model))
        {
            model.battleAttribute.actionBar = 0;
            listener.WriteLog(model.name + " is dead", false);
        }
    }

    bool CheckAllActorStillAlive()
    {
        //TODO More to do
        return (IsValid(enemy1) || IsValid(enemy2)) && IsPlayerAlive();
    }

    public bool IsPlayerAlive()
    {
        return IsValid(player1) || IsValid(player2);
    }

    void OnBattleFinished()
    {
        ClearAllPlayerBuffs();
        listener.WriteLog("Battle completed!", false);
        AllPlayerGetExperience();
    }

    void ClearAllPlayerBuffs()
    {
        ClearAllBuffs(player1);
        ClearAllBuffs(player2);
        ClearAllBuffs(player3);
        ClearAllBuffs(player4);
    }

    void ClearAllBuffs(CharacterModel model)
    {
        if (model != null)
        {
            foreach(BattleBuff buff in model.battleAttribute.buffs)
            {
                buff.BuffOnRemove();
            }
            model.battleAttribute.buffs.Clear();
        }

    }

    void AllPlayerGetExperience()
    {
        if (IsValid(player1))
            player1.GetExperience(GetAllMonsterExperience());
        if (IsValid(player2))
            player2.GetExperience(GetAllMonsterExperience());
        if (IsValid(player3))
            player3.GetExperience(GetAllMonsterExperience());
        if (IsValid(player4))
            player4.GetExperience(GetAllMonsterExperience());
        PlayerSession.GetInstance().SaveSession();
    }

    int GetAllMonsterExperience()
    {
        int total = 0;
        if (enemy1 != null)
            total += enemy1.exp;
        if (enemy2 != null)
            total += enemy2.exp;
        if (enemy3 != null)
            total += enemy3.exp;
        if (enemy4 != null)
            total += enemy4.exp;
        return total;
    }

    void UnloadScene()
    {
        DungeonControl dungeonControl = new DungeonControl();
        dungeonControl.ClickRoomAction(DungeonModel.currentDungeonRoom, DungeonModel.currentTreasureActionPanel, DungeonModel.currentTrapActionPanel, DungeonModel.CurrentLog);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        SceneManager.UnloadSceneAsync(2);
    }
}
