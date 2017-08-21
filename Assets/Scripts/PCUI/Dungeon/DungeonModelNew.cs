using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.ComponentModel;

[XmlRoot("Dungeon")]
public class DungeonInfoNew
{

    [XmlAttribute("Name")]
    public string name;
    [DefaultValue(1)]
    [XmlAttribute("Room")]
    public int allRoom;
    [DefaultValue(0)]
    [XmlAttribute("TreasureRoom")]
    public int maxTreasure;
    [DefaultValue(0)]
    [XmlAttribute("TrapRoom")]
    public int maxTrap;
    [DefaultValue(0)]
    [XmlAttribute("EnemyRoom")]
    public int maxEnemy;
    [XmlAttribute("MinGetTreasure")]
    public int minGet;
    [XmlAttribute("MaxGetTreasure")]
    public int maxGet;
    [XmlAttribute("ChanceGetEquipment")]
    public int chanceGetEq;
    [XmlAttribute("MinGetEquipment")]
    public int minGetEq;
    [XmlAttribute("MaxGetEquipment")]
    public int maxGetEq;
    [XmlAttribute("MinEquipmentLv")]
    public int minEqLv;
    [XmlAttribute("MaxEquipmentLv")]
    public int maxEqLv;
    [XmlArray("EnemyType")]
    [XmlArrayItem("Enemy")]
    public List<EnemyList> enemy;
    [XmlArray("Treasure")]
    [XmlArrayItem("Item")]
    public List<ItemList> item;
    [XmlArray("Equipment")]
    [XmlArrayItem("Equip")]
    public List<EquipList> equip;
    [XmlArray("DungeonBoss")]
    [XmlArrayItem("Boss")]
    public List<Boss> boss;

}

[XmlRoot("Enemy")]
public class EnemyListNew
{
    [XmlAttribute("Id")]
    public string enemyId;
    [XmlAttribute("Lv")]
    public int enemyLv;
    [XmlAttribute("Weight")]
    public int enemyWeight;
    [XmlAttribute("Chance")]
    public int enemyMeetChance;
}

[XmlRoot("Item")]
public class ItemListNew
{
    [XmlAttribute("ItemId")]
    public string itemId;
    [DefaultValue(1)]
    [XmlAttribute("Chance")]
    public int chance;
    [XmlAttribute("AmountMin")]
    public int amountMin;
    [XmlAttribute("AmountMax")]
    public int amountMax;
}

[XmlRoot("Equip")]
public class EquipListNew
{
    [XmlAttribute("Tier")]
    public int tier;
    [XmlAttribute("Chance")]
    public int chance;
}

[XmlRoot("Boss")]
public class BossNew
{
    [XmlAttribute("Id")]
    public string bossId;
    [XmlAttribute("Lv")]
    public int bossLv;
}

public class DungeonModelNew {

    public int roomId;
    public Vector2 roomPosition;

    public DungeonModelNew(int newRoomId, Vector2 newRoomPosition)
    {
        roomId = newRoomId;
        roomPosition = newRoomPosition;
    }
}

public class DungeonCoridorModel
{
    public int coridorId;
    public int coridorDirection;
    public Vector2 coridorOffsetMax;
    public Vector2 coridorOffsetMin;

    public DungeonCoridorModel(int newCoridorId,int newCoridorDirection,Vector2 newCoridorOffsetMax,Vector2 newCoridorOffsetMin)
    {
        coridorId = newCoridorId;
        coridorDirection = newCoridorDirection;
        coridorOffsetMax = newCoridorOffsetMax;
        coridorOffsetMin = newCoridorOffsetMin;
    }
}

public class DungeonSession
{
    static DungeonSession instance;
    List<DungeonModelNew> dungeonModel;
    List<DungeonCoridorModel> dungeonCoridorModel;

    public static DungeonSession GetSession()
    {
        if (instance == null)
            instance = new DungeonSession();
        return instance;
    }

    public List<DungeonModelNew> SetDungeonRoom(List<DungeonModelNew> model)
    {
        dungeonModel= model;
        return dungeonModel;
    }

    public List<DungeonModelNew> GetDungeonRoom()
    {
        return dungeonModel;
    }

    public List<DungeonCoridorModel> SetDungeonCoridor(List<DungeonCoridorModel> model)
    {
        dungeonCoridorModel = model;
        return dungeonCoridorModel;
    }

    public List<DungeonCoridorModel> GetDungeonCoridor()
    {
        return dungeonCoridorModel;
    }
}

public class DungeonManagerNew{
    static DungeonManagerNew instance;
    DungeonInfoNew dungeonInfo;

    public DungeonManagerNew(string Id)
    {
        dungeonInfo= XmlLoader.LoadFromXmlResource<DungeonInfoNew>("Xml/Dungeon/" + Id); ;
    }

    public static DungeonManagerNew GetInstance(string Id)
    {
        if (instance == null)
            instance = new DungeonManagerNew(Id);
        return instance;
    }

    public DungeonInfoNew GetDungeonInfo()
    {
        return dungeonInfo;
    }


}

