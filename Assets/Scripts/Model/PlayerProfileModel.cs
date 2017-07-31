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

    [XmlElement("Items")]
    public string itemString;

    [XmlIgnore]
    public SessionInfo session;

    [XmlElement("Party")]
    public PartyModel party = new PartyModel();

    [XmlIgnore]
    public List<string> itemsId;

    [XmlIgnore]
    public List<int> itemsOwned;

    public CharacterModel GetCharacter(int index)
    {
        if (index >= characters.Count || index < 0)
            return null;
        else
            return characters[index];
    }

    public void ParseItemToList()
    {
        itemsId = new List<string>();
        itemsOwned = new List<int>();
        string[] separated = itemString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in separated)
        {
            string[] splits = item.Split('%');
            itemsId.Add(splits[0]);
            itemsOwned.Add(int.Parse(splits[1]));
        }
    }

    public void ParseItemToText()
    {
        itemString = "";
        int total = itemsId.Count;
        for (int i = 0; i < total; i++)
        {
            if(itemsOwned[i] > 0)
            {
                itemString += "|" + itemsId[i] + "%" + itemsOwned[i];
            }
        }
    }

    public void AddItem(string id, int qty)
    {
        if (itemsId.Contains(id))
        {
            itemsOwned[itemsId.IndexOf(id)] += qty;
        }
        else
        {
            itemsId.Add(id);
            itemsOwned.Add(qty);
        }
    }

    public void RemoveItem(string id, int qty)
    {
        //TODO later
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