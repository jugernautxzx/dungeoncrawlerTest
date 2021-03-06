﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Xml.Serialization;
using UnityEngine.UI;

[XmlRoot("Dungeon")]
public class DungeonInfo {

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
public class EnemyList
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
public class ItemList
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
public class EquipList
{
    [XmlAttribute("Tier")]
    public int tier;
    [XmlAttribute("Chance")]
    public int chance;
}

[XmlRoot("Boss")]
public class Boss
{
    [XmlAttribute("Id")]
    public string bossId;
    [XmlAttribute("Lv")]
    public int bossLv;
}

public class DungeonModel {
    public static int indexCoridor=0;
    public static float maxPanelTopBottom = 0;
    public static float maxPanelRightLeft = 0;
    public static float verticalNormalPosition;
    public static float horizontalNormalPosition;
    public static int playerInRoom = 0;
    public static bool battleWon;
    public static Button[] currentDungeonRoom;
    public static GameObject currentTreasureActionPanel;
    public static GameObject currentTrapActionPanel;
    public static Text currentLog;
    public static Text treasureItem;
    public static RectTransform treasureContent;
    public static Button consumableItem;
    public static RectTransform consumableContent;
    public static RectTransform inventoryContent;
    public static string enemy1;
    public static int lvEnemy1;
    public static string enemy2;
    public static int lvEnemy2;
    public static string enemy3;
    public static int lvEnemy3;
    public static string enemy4;
    public static int lvEnemy4;

}

public class Loot
{
    public string itemId;
    public int amount;
    public ItemType type;
    public int index;

    public Loot(string newItemId, int newAmount, ItemType newType, int newIndex){
        itemId = newItemId;
        amount = newAmount;
        type = newType;
        index = newIndex;
    }


}

public class DungeonManager {

    public static DungeonInfo DungeonLoad(string Id)
    {
        return XmlLoader.LoadFromXmlResource<DungeonInfo>("Xml/Dungeon/" + Id);
    }

}
