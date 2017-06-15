using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("PlayerProfile")]
public class PlayerProfileModel{

    [XmlAttribute("Gold")]
    public int Gold;

    [XmlArray("Characters")]
    [XmlArrayItem("Character")]
    public List<CharacterModel> characters;

    [XmlElement("MainCharacter")]
    public MainCharaModel mainChara;
}

public class MainCharaModel
{

}