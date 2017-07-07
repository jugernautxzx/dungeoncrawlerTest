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
        battleAttribute.hp = Mathf.RoundToInt(level * 1.2f) + Mathf.RoundToInt((((3 * attribute.cons) + attribute.endurance) / 4f) * 15);
        battleAttribute.currHp = battleAttribute.hp;
        battleAttribute.mp = 10;
        battleAttribute.currMp = battleAttribute.mp;
        battleAttribute.stamina = 0;
        battleAttribute.pAtk = attribute.str;
        battleAttribute.pDef = attribute.cons;
        battleAttribute.mAtk = attribute.intel;
        battleAttribute.mDef = attribute.wisdom;
        battleAttribute.speed = attribute.speed;
        battleAttribute.baseMatk = battleAttribute.mAtk;
        battleAttribute.basePAtk = battleAttribute.pAtk;
        battleAttribute.baseMDef = battleAttribute.mDef;
        battleAttribute.basePDef = battleAttribute.pDef;
        battleAttribute.backRow = battleSetting.backRow;
    }

    public void GetExperience(int ex)
    {
        exp += ex;
    }

    public void CalculateEqAttribute()
    {
        if (eqAttribute == null)
            eqAttribute = new Attribute();
        else
            eqAttribute.Reset();
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
    }
}

public class Attribute
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
}

public class BattleAttribute
{
    public int hp;
    public int currHp;
    public int mp;
    public int currMp;
    public int currStamina;
    public int stamina;
    public int basePAtk;
    public int baseMatk;
    public int baseMDef;
    public int baseSpeed;
    public int basePDef;
    public int pAtk;
    public int pDef;
    public int mAtk;
    public int mDef;
    public int speed;
    public bool backRow;
    public int morale;
    public int actionBar;
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