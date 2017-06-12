public class ActiveUse
{

    BattleManager bm;
    ActiveSkillManager activeManager;
    CharacterModel turnTaker;
    BattleCalculator calculate;

    public ActiveUse(BattleManager bManager)
    {
        bm = bManager;
        activeManager = ActiveSkillManager.GetInstance();
        calculate = new BattleCalculator();
    }

    public void SetTurn(CharacterModel turn)
    {
        turnTaker = turn;
    }

    public void PlayerActorSkillTarget(int index, bool isPlayerSide, int skillIndex)
    {
        foreach (ActiveEffect effect in activeManager.GetActive(turnTaker.actives[skillIndex]).effects)
        {
            ActorSkillEffectTarget(bm.GetCharacter(index, isPlayerSide), effect);
        }
        turnTaker.battleAttribute.actionBar -= 2000;
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
            if (bm.IsValid(bm.GetEnemy(i)))
                if (effect.row == Row.all)
                {
                    ActorGetSkillEffect(bm.GetEnemy(i), effect);
                }
                else if (effect.row == Row.front)
                {
                    if (!bm.GetEnemy(i).battleAttribute.backRow)
                        ActorGetSkillEffect(bm.GetEnemy(i), effect);
                }
                else
                {
                    if (bm.GetEnemy(i).battleAttribute.backRow)
                        ActorGetSkillEffect(bm.GetEnemy(i), effect);
                }
        }
    }

    void AllPartyGetSkillEffect(ActiveEffect effect)
    {
        for (int i = 0; i < 4; i++)
        {
            if (bm.IsValid(bm.GetPlayer(i)))
                if (effect.row == Row.all)
                {
                    ActorGetSkillEffect(bm.GetPlayer(i), effect);
                }
                else if (effect.row == Row.front)
                {
                    if (!bm.GetPlayer(i).battleAttribute.backRow)
                        ActorGetSkillEffect(bm.GetEnemy(i), effect);
                }
                else
                {
                    if (bm.GetPlayer(i).battleAttribute.backRow)
                        ActorGetSkillEffect(bm.GetEnemy(i), effect);
                }
        }
    }

    void ActorGetSkillEffect(CharacterModel target, ActiveEffect effect)
    {//TODO add another skill effects
        switch (effect.type)
        {
            case EffectType.damage:
                ActiveEffectDamage(target, effect);
                break;
            case EffectType.buff:
                break;
            case EffectType.debuff:
                break;
            case EffectType.heal:
                break;
            default:
                break;
        }
        switch (effect.special)
        {
            case SpecialEffect.none:
                break;
            case SpecialEffect.lifeleech:
                break;
            case SpecialEffect.poison:
                ActiveEffectBuff(target, SpecialPoison());
                break;
            default:
                break;
        }
        bm.UpdateAllTeam();
    }

    void ActiveEffectDamage(CharacterModel target, ActiveEffect effect)
    {
        int damage = calculate.DoSkillDamageCalc(turnTaker, target, effect);
        bm.WriteLog(turnTaker.name + " attack " + target.name + " for " + damage + " damage.");
        target.battleAttribute.ModifyHp(-damage);
        if (target.battleAttribute.currHp < 0)
            target.battleAttribute.currHp = 0;
    }

    void ActiveEffectBuff(CharacterModel target, ActiveEffect effect)
    {
        BattleBuff buff = BuffManager.CreateBuff(effect.formula, effect.formulaParam);
        buff.model = target;
        buff.log = bm;
        target.battleAttribute.buffs.Add(buff);
        buff.BuffOnGet();
    }

    ActiveEffect SpecialPoison()
    {
        ActiveEffect effect = new ActiveEffect();
        effect.formula = "PoisonBuff";
        effect.formulaParam = "2|Poison|<color=#00ff00>PSN</color>|InfoPoison|3";
        return effect;
    }

}
