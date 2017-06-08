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
    void WriteLog(string log, bool clear);
    void StartEnemyCoroutine(IEnumerator coroutine);
}

public class BattleManager
{

    CharacterModel player1, player2, player3, player4, enemy1, enemy2, enemy3, enemy4;

    CharacterModel turnTaker;
    int turnTakerIndex;

    bool timer, allActorsAlive;
    BattleInterface listener;
    BattleCalculator calculate;
    ActiveSkillManager activeSkill;
    PassiveManager passiveSkill;

    public BattleManager(BattleInterface listener)
    {
        this.listener = listener;
        calculate = new BattleCalculator();
        activeSkill = ActiveSkillManager.GetInstance();
        passiveSkill = new PassiveManager();
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
        player1.attribute.speed = 6;
        player1.actives.Add("Sweep");
        player1.actives.Add("Sweep2");
        //player2 = Debugger.GenerateCharacterModel("Ramboman");
        //player2.isMainCharacter = true;
        //player2.attribute.speed = 2;
        //player2.battleSetting.backRow = true;
        enemy1 = Debugger.GenerateCharacterModel("Skeleton 1");
        enemy1.attribute.speed = 5;
        enemy2 = Debugger.GenerateCharacterModel("Skeleton 2");
        enemy2.attribute.speed = 5;
        enemy3 = Debugger.GenerateCharacterModel("Skeleton 3");
        enemy3.attribute.speed = 5;
        enemy3.battleSetting.backRow = true;
        enemy4 = Debugger.GenerateCharacterModel("Skeleton 4");
        enemy4.attribute.speed = 5;
        enemy4.battleSetting.backRow = true;
        //
        player1.GenerateBasicBattleAttribute();
        player1.battleAttribute.physAttack = 10;
        //player2.GenerateBasicBattleAttribute();
        enemy1.GenerateBasicBattleAttribute();
        enemy2.GenerateBasicBattleAttribute();
        enemy3.GenerateBasicBattleAttribute();
        enemy4.GenerateBasicBattleAttribute();
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
        return activeSkill.GetActive(turnTaker.actives[index]).info;
    }

    public void RequestSkillTarget(int skillIndex)
    {
        SetSkillTarget(activeSkill.GetActive(turnTaker.actives[skillIndex]));
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

    void ActorAttackTarget(int index, bool isPlayerSide)
    {
        turnTaker.battleAttribute.actionBar -= 2000;
        ActorSingleTargetAttack(GetCharacter(index, isPlayerSide));
    }

    void ActorSingleTargetAttack(CharacterModel target)
    {
        int damage = calculate.DoNormalAttack(turnTaker, target);
        listener.WriteLog(turnTaker.name + " attack " + target.name + " for " + damage + " damage.", false);
        target.battleAttribute.currHp -= damage;
        if (target.battleAttribute.currHp < 0)
            target.battleAttribute.currHp = 0;
        UpdateAllTeam();
    }
    //------------------------------- END OF NORMAL ATTACK ---------------------------------------------------------------------------------------------------
    //------------------------------- ACTIVE SKILLS ----------------------------------------------------------------------------------------------------------

    public void PlayerActorSkillTarget(int index, bool isPlayerSide, int skillIndex)
    {
        listener.WriteLog("", true);
        foreach (ActiveEffect effect in activeSkill.GetActive(turnTaker.actives[skillIndex]).effects)
        {
            ActorSkillEffectTarget(GetCharacter(index, isPlayerSide), effect);
        }
        turnTaker.battleAttribute.actionBar -= 2000;
        timer = true;
    }

    void ActorSkillEffectTarget(CharacterModel target, ActiveEffect effect)
    {
        switch (effect.target)
        {
            case Target.self:
                ActorGetSkillEffect(turnTaker, effect);
                break;
            case Target.target:
                ActorGetSkillEffect(target, effect);
                break;
            case Target.allenemy:
                AllEnemyGetSkillEffect(effect);
                break;
            case Target.allparty:
                AllPartyGetSkillEffect(effect);
                break;
            case Target.everyone:
                AllEnemyGetSkillEffect(effect);
                AllPartyGetSkillEffect(effect);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    void AllEnemyGetSkillEffect(ActiveEffect effect)
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsValid(GetEnemy(i)))
                if (effect.row == Row.all)
                {
                    ActorGetSkillEffect(GetEnemy(i), effect);
                }
                else if (effect.row == Row.front)
                {
                    if (!GetEnemy(i).battleAttribute.backRow)
                        ActorGetSkillEffect(GetEnemy(i), effect);
                }
                else
                {
                    if (GetEnemy(i).battleAttribute.backRow)
                        ActorGetSkillEffect(GetEnemy(i), effect);
                }
        }
    }

    void AllPartyGetSkillEffect(ActiveEffect effect)
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsValid(GetPlayer(i)))
                if (effect.row == Row.all)
                {
                    ActorGetSkillEffect(GetPlayer(i), effect);
                }
                else if (effect.row == Row.front)
                {
                    if (!GetPlayer(i).battleAttribute.backRow)
                        ActorGetSkillEffect(GetEnemy(i), effect);
                }
                else
                {
                    if (GetPlayer(i).battleAttribute.backRow)
                        ActorGetSkillEffect(GetEnemy(i), effect);
                }
        }
    }

    void ActorGetSkillEffect(CharacterModel target, ActiveEffect effect)
    {//TODO add another skill effects
        int damage = calculate.DoSkillDamageCalc(turnTaker, target, effect);
        listener.WriteLog(turnTaker.name + " attack " + target.name + " for " + damage + " damage.", false);
        target.battleAttribute.currHp -= damage;
        if (target.battleAttribute.currHp < 0)
            target.battleAttribute.currHp = 0;
        UpdateAllTeam();
    }

    //------------------------------- END OF ACTIVE SKILLS ----------------------------------------------------------------------------------------------------------

    IEnumerator BattleTimer()
    {
        yield return new WaitForSeconds(1f);
        while (allActorsAlive)
        {
            yield return new WaitUntil(() => timer);
            yield return new WaitForSeconds(0.02f);
            AllActorsActionBarFill();
            listener.UpdateTimer(GetTimer(player1), GetTimer(player2), GetTimer(player3), GetTimer(player4));
            while (AnyActorCanMove())
            {
                timer = false;
                ActorTakeTurn();
                yield return new WaitUntil(() => timer);
            }
        }
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
        if (actor == null)
            return;
        if (actor.battleAttribute.hp <= 0)
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
        if (actor == null)
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
        if (!IsValid(enemy))
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

    bool IsValid(CharacterModel model)
    {
        if (model == null)
            return false;
        if (model.battleAttribute.currHp == 0)
            return false;
        return true;
    }

    IEnumerator AITakeTurn()
    {
        Debug.Log("AI Take turn : " + turnTaker.name);
        yield return new WaitForSeconds(0.5f);
        ActorAttackTarget(0, true);
        yield return new WaitForSeconds(0.25f);
        timer = true;
    }

    CharacterModel GetCharacter(int index, bool isPlayerSide)
    {
        if (isPlayerSide)
            return GetPlayer(index);
        else
            return GetEnemy(index);
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
