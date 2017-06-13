using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("ActiveSkills")]
public class ActiveSkillList : RealListWithDictionary<ActiveSkill>
{
}

[XmlRoot("Active")]
public class ActiveSkill : ClassWithId
{
    [XmlAttribute("Name")]
    public string name;
    [XmlAttribute("Mana")]
    public string mana;
    [XmlAttribute("Acc")]
    public int acc;
    [XmlAttribute("Stamina")]
    public string stamina;
    [XmlAttribute("Weapons")]
    public string weapons;
    [XmlAttribute("Targetable")]
    public Targetable target;
    [XmlAttribute("Row")]
    public Row row;
    [XmlArray("Effects")]
    [XmlArrayItem("Effect")]
    public List<ActiveEffect> effects;
    [XmlElement("Info")]
    public string info;
}

public class ActiveEffect
{
    [XmlAttribute("Target")]
    public Target target;
    [XmlAttribute("Row")]
    public Row row;
    [XmlAttribute("Type")]
    public EffectType type;
    [XmlAttribute("Element")]
    public int element;
    [XmlAttribute("Formula")]
    public string formula;
    [XmlAttribute("FormulaParam")]
    public string formulaParam;
    [XmlAttribute("Special")]
    public SpecialEffect special;
    [XmlAttribute("SpecialParam")]
    public string specialVal;
}

public class ActiveSkillManager
{
    ActiveSkillList list;
    static ActiveSkillManager instance;

    public static ActiveSkillManager GetInstance()
    {
        if (instance == null)
            instance = new ActiveSkillManager();
        return instance;
    }

    public ActiveSkillManager()
    {
        list = XmlLoader.LoadFromXmlResource<ActiveSkillList>("Xml/ActiveSkills");
        list.GenerateDictionary();
        Debug.Log("Successfully loaded: " + list.list.Count);
    }

    public ActiveSkill GetActive(string id)
    {
        return list.Get(id);
    }
}