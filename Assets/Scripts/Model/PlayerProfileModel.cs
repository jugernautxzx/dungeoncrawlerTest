using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("PlayerProfile")]
public class PlayerProfileModel
{

    [XmlAttribute("Gold")]
    public int Gold;

    [XmlArray("Characters")]
    [XmlArrayItem("Character")]
    public List<CharacterModel> characters;

    [XmlElement("MainCharacter")]
    public MainCharaModel mainChara;

    [XmlIgnore]
    public SessionInfo session;

    [XmlElement("Party")]
    public PartyModel party = new PartyModel();

    public CharacterModel GetCharacter(int index)
    {
        if (index >= characters.Count || index < 0)
            return null;
        else
            return characters[index];
    }
}



public class PartyModel
{
    [XmlAttribute("Member1")]
    public int member1 = -1;
    [XmlAttribute("Member2")]
    public int member2 = -1;
    [XmlAttribute("Member3")]
    public int member3 = -1;
}

public class MainCharaModel
{
    [XmlAttribute("Fame")]
    public int fame;
    [XmlAttribute("Leadership")]
    public int leadership;
}

public class SessionInfo
{

}