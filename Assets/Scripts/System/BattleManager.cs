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
    List<CharacterModel> playerList, enemyList;

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
        playerList = new List<CharacterModel>();
        enemyList = new List<CharacterModel>();
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
        listener.UpdatePlayerTeam(GetPlayer(0), GetPlayer(1), GetPlayer(2), GetPlayer(3));
        listener.UpdateEnemyTeam(GetEnemy(0), GetEnemy(1), GetEnemy(2), GetEnemy(3));
    }

    void InitiateActors()
    {
        //TODO Debugging purpose
        playerList.Add(PlayerSession.GetProfile().characters[0]);
        playerList[0].GenerateBasicBattleAttribute();
        if (PlayerSession.GetProfile().party.member1 > 0)
        {
            playerList.Add(PlayerSession.GetProfile().characters[PlayerSession.GetProfile().party.member1]);
        }
        if (PlayerSession.GetProfile().party.member2 > 0)
        {
            playerList.Add(PlayerSession.GetProfile().characters[PlayerSession.GetProfile().party.member2]);
        }
        if (PlayerSession.GetProfile().party.member3 > 0)
        {
            playerList.Add(PlayerSession.GetProfile().characters[PlayerSession.GetProfile().party.member3]);
        }

        enemyList.Add(MonsterLoader.LoadMonsterData(DungeonModel.enemy1, DungeonModel.lvEnemy1));
        //enemy2 = MonsterLoader.LoadMonsterData(DungeonModel.enemy2, DungeonModel.lvEnemy2);
        //enemy3 = MonsterLoader.LoadMonsterData(DungeonModel.enemy3, DungeonModel.lvEnemy3);
        //enemy4 = MonsterLoader.LoadMonsterData(DungeonModel.enemy4, DungeonModel.lvEnemy4);
        InitEnemy(enemyList[0]);
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
            listener.UpdateTimer(GetTimer(GetPlayer(0)), GetTimer(GetPlayer(1)), GetTimer(GetPlayer(2)), GetTimer(GetPlayer(3)));
            while (AnyActorCanMove() && CheckAllActorStillAlive())
            {
                timer = false;
                ActorTakeTurn();
                yield return new WaitUntil(() => timer);
                listener.UpdateTimer(GetTimer(GetPlayer(0)), GetTimer(GetPlayer(1)), GetTimer(GetPlayer(2)), GetTimer(GetPlayer(3)));
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
        playerList.ForEach(player => ActorActionBarFill(player));
        enemyList.ForEach(enemy => ActorActionBarFill(enemy));
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

    void TurnTakerBuffOnTurn2()
    {
        turnTaker.battleAttribute.buffs.ForEach(buff => buff.CharaTakeTurn());
        turnTaker.battleAttribute.buffs.RemoveAll(buff => buff.IsExpired());
        WriteActorStillAlive(turnTaker);
    }

    void TurnTakerBuffOnTurn()
    {
        TurnTakerBuffOnTurn2();
        //TODO Remove this later
        //List<BattleBuff> toRemove = new List<BattleBuff>();
        //foreach (BattleBuff buff in turnTaker.battleAttribute.buffs)
        //{
        //    buff.CharaTakeTurn();
        //    if (buff.IsExpired())
        //        toRemove.Add(buff);
        //}
        //foreach (BattleBuff buff in toRemove)
        //{
        //    turnTaker.battleAttribute.buffs.Remove(buff);
        //}
        //WriteActorStillAlive(turnTaker);
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
        if (index < playerList.Count)
            return playerList[index];
        else
            return null;
    }

    public CharacterModel GetEnemy(int index)
    {
        if (index < enemyList.Count)
            return enemyList[index];
        else
            return null;
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
        return IsAnyEnemyAlive() && IsAnyPlayerAlive();
    }

    bool IsAnyEnemyAlive()
    {
        return IsValid(GetEnemy(0)) || IsValid(GetEnemy(1)) || IsValid(GetEnemy(2)) || IsValid(GetEnemy(3));
    }

    public bool IsAnyPlayerAlive()
    {
        return IsValid(GetPlayer(0)) || IsValid(GetPlayer(1)) || IsValid(GetPlayer(2)) || IsValid(GetPlayer(3));
    }

    void OnBattleFinished()
    {
        ClearAllPlayerBuffs();
        listener.WriteLog("Battle completed!", false);
        AllPlayerGetExperience();
    }

    void ClearAllPlayerBuffs()
    {
        playerList.ForEach(player => ClearAllBuffs(player));
    }

    void ClearAllBuffs(CharacterModel model)
    {
        model.battleAttribute.buffs.ForEach(buff => buff.BuffOnRemove());
        model.battleAttribute.buffs.Clear();
    }

    void AllPlayerGetExperience()
    {
        playerList.ForEach(player => player.GetExperience(GetAllMonsterExperience()));
        PlayerSession.GetInstance().SaveSession();
    }

    int GetAllMonsterExperience()
    {
        int total = 0;
        enemyList.ForEach(enemy => total += enemy.exp);
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
