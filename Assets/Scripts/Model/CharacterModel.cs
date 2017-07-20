using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Character")]
public class CharacterModel
{

    [XmlAttribute("IsMainCharacter")]
    public bool isMainCharacter;
    [XmlAttribute("MonsterId")]
    public string monsterId;
    [XmlAttribute("Name")]
    public string name;
    [XmlAttribute("Level")]
    public int level;
    [XmlAttribute("Experience")]
    public int exp;
    [DefaultValue(false)]
    [XmlAttribute("LevelUp")]
    public bool levelUp;
    [XmlElement("Attribute")]
    public Attribute attribute;
    [XmlElement("Elemental")]
    public ElementAttribute elemental;
    [XmlIgnore]
    public BattleAttribute battleAttribute;
    [XmlIgnore]
    public Attribute eqAttribute;
    [XmlIgnore]
    public BattleAttribute equipBAttribute;
    [XmlElement("BattleSetting")]
    public BattleSetting battleSetting;
    [XmlArray("Actives")]
    [XmlArrayItem("Active")]
    public List<string> actives;
    [XmlArray("Passives")]
    [XmlArrayItem("Passive")]
    public List<string> passives;
    [XmlArray("Traits")]
    [XmlArrayItem("Trait")]
    public List<string> traits;
    [XmlArray("LearnedActives")]
    [XmlArrayItem("Active")]
    public List<string> learnActive;
    [XmlArray("LearnedPassive")]
    [XmlArrayItem("Passive")]
    public List<string> learnPassive;
    [XmlIgnore]
    public BaseMonster monster;

    public void GenerateBasicBattleAttribute()
    {
        if (battleAttribute != null)
            return;
        //TODO NOTE THIS IS STILL BETA AND WORK IN PROGRESS, USED FOR TESTING PURPOSE NO BALANCING
        battleAttribute = new BattleAttribute();
        CalculateEqAttribute();
        battleAttribute.hp = Mathf.RoundToInt(level * 1.2f) + Mathf.RoundToInt((((3 * attribute.cons) + attribute.endurance) / 4f) * 15);
        battleAttribute.currHp = battleAttribute.hp;
        battleAttribute.mp = 10;
        battleAttribute.currMp = battleAttribute.mp;
        battleAttribute.stamina = 0;
        battleAttribute.pAtk = attribute.str + eqAttribute.str + equipBAttribute.basePAtk;
        battleAttribute.pDef = attribute.cons + eqAttribute.cons + equipBAttribute.basePDef;
        battleAttribute.mAtk = attribute.intel + eqAttribute.intel + equipBAttribute.baseMatk;
        battleAttribute.mDef = attribute.wisdom + eqAttribute.wisdom + equipBAttribute.baseMDef;
        battleAttribute.speed = attribute.speed + eqAttribute.speed + equipBAttribute.speed;
        battleAttribute.baseMatk = battleAttribute.mAtk;
        battleAttribute.basePAtk = battleAttribute.pAtk;
        battleAttribute.baseMDef = battleAttribute.mDef;
        battleAttribute.basePDef = battleAttribute.pDef;
        battleAttribute.backRow = battleSetting.backRow;
    }

    public void GetExperience(int ex)
    {
        exp += ex;
        //TODO Calculating exp needed for next level
        levelUp = LevelCalculator.CalculateLevelUp(exp, level);
    }

    public void CalculateEqAttribute()
    {
        if (eqAttribute == null)
        {
            eqAttribute = new Attribute();
            equipBAttribute = new BattleAttribute();
        }

        else
        {
            eqAttribute.Reset();
            equipBAttribute.Reset();
        }
        AddAllEquipmentAttrib(PlayerSession.GetEquipment(battleSetting.mainHand));
        AddAllEquipmentAttrib(PlayerSession.GetEquipment(battleSetting.offHand));
        AddAllEquipmentAttrib(PlayerSession.GetEquipment(battleSetting.head));
        AddAllEquipmentAttrib(PlayerSession.GetEquipment(battleSetting.body));
        AddAllEquipmentAttrib(PlayerSession.GetEquipment(battleSetting.acc1));
        AddAllEquipmentAttrib(PlayerSession.GetEquipment(battleSetting.acc2));
    }

    void AddAllEquipmentAttrib(Equipment model)
    {
        if (model == null)
            return;
        eqAttribute.str += model.attribute.str;
        eqAttribute.agi += model.attribute.agi;
        eqAttribute.cons += model.attribute.cons;
        eqAttribute.endurance += model.attribute.endurance;
        eqAttribute.intel += model.attribute.intel;
        eqAttribute.wisdom += model.attribute.wisdom;
        eqAttribute.speed += model.attribute.speed;
        equipBAttribute.basePAtk += model.battle.basePAtk;
        equipBAttribute.basePDef += model.battle.basePDef;
        equipBAttribute.baseMatk += model.battle.baseMatk;
        equipBAttribute.baseMDef += model.battle.baseMDef;
        equipBAttribute.hp += model.battle.hp;
        equipBAttribute.mp += model.battle.mp;
        equipBAttribute.stamina += model.battle.stamina;
    }
}

public class Attribute : ICloneable
{
    [DefaultValue(0)]
    [XmlElement("Str")]//PAtk PDef
    public int str;
    [DefaultValue(0)]
    [XmlElement("Agi")]//Spd StaRegen dodge
    public int agi;
    [DefaultValue(0)]
    [XmlElement("Intel")]//MAtk Mana ManaRegen
    public int intel;
    [DefaultValue(0)]
    [XmlElement("Endurance")]//HP stamina
    public int endurance;
    [DefaultValue(0)]
    [XmlElement("Wisdom")]//Mana MDef ManaRegen
    public int wisdom;
    [DefaultValue(0)]
    [XmlElement("Constitution")]//PDef HP
    public int cons;
    [DefaultValue(0)]
    [XmlElement("Speed")]
    public int speed;

    public void Reset()
    {
        str = 0;
        agi = 0;
        intel = 0;
        endurance = 0;
        wisdom = 0;
        speed = 0;
        cons = 0;
    }

    public object Clone()
    {
        Attribute attr = new Attribute();
        attr.str = str;
        attr.agi = agi;
        attr.intel = intel;
        attr.endurance = endurance;
        attr.wisdom = wisdom;
        attr.cons = cons;
        attr.speed = speed;
        return attr;
    }
}

public class BattleAttribute
{
    [DefaultValue(0)]
    [XmlAttribute("HP")]
    public int hp;
    [XmlIgnore]
    public int currHp;
    [DefaultValue(0)]
    [XmlAttribute("MP")]
    public int mp;
    [XmlIgnore]
    public int currMp;
    [XmlIgnore]
    public int currStamina;
    [DefaultValue(0)]
    [XmlAttribute("Stamina")]
    public int stamina;
    [DefaultValue(0)]
    [XmlAttribute("PAtk")]
    public int basePAtk;
    [DefaultValue(0)]
    [XmlAttribute("MAtk")]
    public int baseMatk;
    [DefaultValue(0)]
    [XmlAttribute("MDef")]
    public int baseMDef;
    [XmlIgnore]
    public int baseSpeed;
    [DefaultValue(0)]
    [XmlAttribute("PDef")]
    public int basePDef;
    [XmlIgnore]
    public int pAtk;
    [XmlIgnore]
    public int pDef;
    [XmlIgnore]
    public int mAtk;
    [XmlIgnore]
    public int mDef;
    [XmlIgnore]
    public int speed;
    [XmlIgnore]
    public bool backRow;
    [XmlIgnore]
    public int morale;
    [XmlIgnore]
    public int actionBar;
    [XmlIgnore]
    public List<BattleBuff> buffs = new List<BattleBuff>();

    public void ModifyHp(int number)
    {
        currHp += number;
        if (currHp > hp)
            currHp = hp;
        else if (currHp < 0)
            currHp = 0;
    }

    public void ModifyMp(int number)
    {
        currMp += number;
        if (currMp > mp)
            currMp = mp;
        else if (currMp < 0)
            currMp = 0;
    }

    public void ModifyStamina(int number)
    {
        currStamina += number;
        if (currStamina > stamina)
            currStamina = stamina;
        else if (currStamina < 0)
            currStamina = 0;
    }

    public void Reset()
    {
        hp = 0;
        mp = 0;
        stamina = 0;
        basePAtk = 0;
        baseMatk = 0;
        basePDef = 0;
        baseMDef = 0;
    }
}

public class BattleSetting
{
    [XmlAttribute("Row")]
    public bool backRow;
    [XmlAttribute("PlayerCharacter")]
    public bool isPlayerCharacter;
    [DefaultValue(-1)]
    [XmlAttribute("MainHand")]
    public int mainHand = -1;
    [DefaultValue(-1)]
    [XmlAttribute("OffHand")]
    public int offHand = -1;
    [DefaultValue(-1)]
    [XmlAttribute("Head")]
    public int head = -1;
    [DefaultValue(-1)]
    [XmlAttribute("Body")]
    public int body = -1;
    [DefaultValue(-1)]
    [XmlAttribute("Acc1")]
    public int acc1 = -1;
    [DefaultValue(-1)]
    [XmlAttribute("Acc2")]
    public int acc2 = -1;
}

public class ElementAttribute
{
    [DefaultValue(0)]
    [XmlElement("Fire")]
    public int fire;
    [DefaultValue(0)]
    [XmlElement("Water")]
    public int water;
    [DefaultValue(0)]
    [XmlElement("Wind")]
    public int wind;
    [DefaultValue(0)]
    [XmlElement("Earth")]
    public int earth;
}