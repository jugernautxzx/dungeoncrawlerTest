
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("PassiveSkills")]
public class PassiveSkillList : RealListWithDictionary<PassiveSkill>
{
}

public class PassiveSkill : ClassWithId{
    [XmlAttribute("Name")]
    public string name;
}

public class PassiveManager
{
    PassiveSkillList list;

    public PassiveManager()
    {
        list = XmlLoader.LoadFromXmlResource<PassiveSkillList>("Xml/PassiveSkills");
        list.GenerateDictionary();
    }
}
