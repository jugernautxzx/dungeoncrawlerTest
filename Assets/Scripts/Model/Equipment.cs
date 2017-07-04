using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerEquipments : RealListWithDictionary<Equipment>
{

}

public class Equipment : ClassWithId {

    [XmlAttribute("Slot")]
    public EqSlot slot;
    [XmlAttribute("Name")]
    public string name;
    [XmlAttribute("Used")]
    public bool isUsed;
    [DefaultValue(Weapon.None)]
    [XmlAttribute("WeaponType")]
    public Weapon weapon;
    [XmlElement("Attribute")]
    public Attribute attribute;
    [XmlElement("Bonus")]
    public BonusAttribute bonus;
}

public class BonusAttribute
{
    [DefaultValue(0)]
    [XmlAttribute("HP")]
    public int hp;
    [DefaultValue(0)]
    [XmlAttribute("Attack")]
    public int attack;
    [DefaultValue(0)]
    [XmlAttribute("Defense")]
    public int defense;
}

