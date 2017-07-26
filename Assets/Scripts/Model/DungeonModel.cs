using System.Collections;
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
    [XmlArray("EnemyType")]
    [XmlArrayItem("Enemy")]
    public List<EnemyList> enemy;

}

[XmlRoot("Enemy")]
public class EnemyList
{
    [XmlAttribute("Id")]
    public string enemyId;
    [XmlAttribute("Lv")]
    public int enemyLv;
}

public class DungeonModel {
    public static int IndexCoridor=0;
    public static float MaxPanelTopBottom = 0;
    public static float MaxPanelRightLeft = 0;
    public static float VerticalNormalPosition;
    public static float HorizontalNormalPosition;
    public static int PlayerInRoom = 0;
    public static bool battleWon;
    public static Button[] currentDungeonRoom;
    public static GameObject currentTreasureActionPanel;
    public static GameObject currentTrapActionPanel;
    public static string enemy1;
    public static int lvEnemy1;
    public static string enemy2;
    public static int lvEnemy2;
    public static string enemy3;
    public static int lvEnemy3;
    public static string enemy4;
    public static int lvEnemy4;

}

public class DungeonManager {

    public static DungeonInfo DungeonLoad(string Id)
    {
        return XmlLoader.LoadFromXmlResource<DungeonInfo>("Xml/Dungeon/" + Id);
    }

}