using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEngine.UI;

[XmlRoot("Dungeon")]
public class DungeonInfo {

    [XmlAttribute("Name")]
    public string name;
    [XmlAttribute("Room")]
    public int allRoom;

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
}

public class DungeonManager {

    public static DungeonInfo DungeonLoad(string Id)
    {
        DungeonInfo dungeon= XmlLoader.LoadFromXmlResource<DungeonInfo>("Xml/Dungeon/" + Id);
        return dungeon;
    }

}