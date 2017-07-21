using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Dungeons")]
public class DungeonInfo {
    [XmlAttribute("Name")]
    public string name;
}

public class DungeonModel {

    public int allRoom = 30; //Total Dungeon Room
    public static int IndexCoridor=0;
    public static float MaxPanelTopBottom = 0;
    public static float MaxPanelRightLeft = 0;
    public static float VerticalNormalPosition;
    public static float HorizontalNormalPosition;
    public static int PlayerInRoom = 0;
}

public class DungeonManager {
    
    public DungeonManager()
    {
        XmlLoader.LoadFromXmlResource<DungeonInfo>("Xml/DungeonList");
    }

}