using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEngine.UI;

[XmlRoot("Dungeon")]
public class Dungeon
{
    [XmlArray("Stages")]
    [XmlArrayItem("Stage")]
    public List<UIDungeonInfo> stage;
}

[XmlRoot("Stage")]
public class UIDungeonInfo
{
    [XmlAttribute("Id")]
    public string stageId;
    [XmlAttribute("Name")]
    public string stageName;
    [XmlAttribute("LvMin")]
    public int stageLvMin;
    [XmlAttribute("LvMax")]
    public int stageLvMax;
    [XmlArray("SubStages")]
    [XmlArrayItem("SubStage")]
    public List<UILevelInfo> levels;
}

[XmlRoot("SubStage")]
public class UILevelInfo
{
    [XmlAttribute("Id")]
    public string subStageId;
    [XmlAttribute("Name")]
    public string subStageName;
    [XmlAttribute("Lv")]
    public int subStageLv;
}

public class LoadDungeonInfo {
    public static Dungeon DungeonLoadInfo()
    {
        return XmlLoader.LoadFromXmlResource<Dungeon>("Xml/DungeonList");
    }
}


public class DungeonSelectUI : MonoBehaviour {

    public GameObject dungeonList;
    public GameObject levelList;
    public Dungeon dungeonListInfo;

    
    //List<UIDungeonInfo> availableDungeons;

	// Use this for initialization
	void Start () {
        dungeonListInfo = LoadDungeonInfo.DungeonLoadInfo();
        AddDungeonList();
        
	}

    public void AddDungeonList()
    {
        GameObject dungeon = new GameObject();

        foreach (UIDungeonInfo dungeonInfo in dungeonListInfo.stage)
        {
            dungeon = Instantiate(dungeonList);
            dungeon.transform.SetParent(gameObject.transform.GetChild(0),false);
            dungeon.transform.GetChild(0).GetComponent<Text>().text = dungeonInfo.stageName;
            dungeon.transform.GetChild(1).GetComponent<Text>().text = dungeonInfo.stageLvMin.ToString();

        }
    }
	
}
