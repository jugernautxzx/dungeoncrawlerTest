using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Character")]
public class CharacterModel{

    [XmlAttribute("IsMainCharacter")]
    public bool isMainCharacter;
    [XmlAttribute("Name")]
    public string name;
    [XmlAttribute("Level")]
    public int level;
    [XmlAttribute("HP")]
    public int baseHP;
    [XmlAttribute("MP")]
    public int baseMP;
    [XmlAttribute("Stamina")]
    public int baseStamina;
    [XmlElement("Attribute")]
    public Attribute attribute;
    [XmlElement("Elemental")]
    public ElementAttribute elemental;
    [XmlIgnore]
    public BattleAttribute battleAttribute;
    [XmlElement("BattleSetting")]
    public BattleSetting battleSetting;
    [XmlArray("Actives")]
    [XmlArrayItem("Active")]
    public List<string> actives;
    [XmlArray("Passives")]
    [XmlArrayItem("Passive")]
    public List<string> passives;

    public void GenerateBasicBattleAttribute()
    {
        //TODO NOTE THIS IS STILL BETA AND WORK IN PROGRESS, USED FOR TESTING PURPOSE NO BALANCING
        battleAttribute = new BattleAttribute();
        battleAttribute.hp = baseHP + Mathf.RoundToInt(level * 1.2f) + attribute.endurance + attribute.cons;
        battleAttribute.currHp = battleAttribute.hp;
        battleAttribute.mp = 10;
        battleAttribute.currMp = battleAttribute.mp;
        battleAttribute.stamina = 0;
        battleAttribute.physAttack = attribute.str;
        battleAttribute.physDef = attribute.cons;
        battleAttribute.magicAttack = attribute.intel;
        battleAttribute.magicDef = attribute.wisdom;
        battleAttribute.speed = attribute.speed;
        battleAttribute.backRow = battleSetting.backRow;
    }
}

public class Attribute
{
    [XmlElement("Str")]//PAtk PDef
    public int str;
    [XmlElement("Agi")]//Spd StaRegen dodge
    public int agi;
    [XmlElement("Intel")]//MAtk Mana ManaRegen
    public int intel;
    [XmlElement("Endurance")]//HP stamina
    public int endurance;
    [XmlElement("Wisdom")]//Mana MDef ManaRegen
    public int wisdom;
    [XmlElement("Constitution")]//PDef HP
    public int cons;
    [XmlElement("Speed")]
    public int speed;
}

public class BattleAttribute
{
    public int hp;
    public int currHp;
    public int mp;
    public int currMp;
    public int stamina;
    public int physAttack;
    public int physDef;
    public int magicAttack;
    public int magicDef;
    public int speed;
    public bool backRow;
    public int morale;
    public int actionBar;
}

public class BattleSetting
{
    [XmlAttribute("Row")]
    public bool backRow;
    [XmlAttribute("Weapon")]
    public int weapon;
    [XmlAttribute("PlayerCharacter")]
    public bool isPlayerCharacter;
}

public class ElementAttribute
{
    [XmlElement("Fire")]
    public int fire;
    [XmlElement("Water")]
    public int water;
    [XmlElement("Wind")]
    public int wind;
    [XmlElement("Earth")]
    public int earth;
}