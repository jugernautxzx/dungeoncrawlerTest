using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Dungeon")]
public class DungeonSelectModel
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

public class DungeonSelectManager
{
    static DungeonSelectManager instance;
    DungeonSelectModel model;

    public DungeonSelectManager()
    {
         model = XmlLoader.LoadFromXmlResource<DungeonSelectModel>("Xml/DungeonList");
    }

    public static DungeonSelectManager GetInstance()
    {
        if (instance == null)
            instance = new DungeonSelectManager();
        return instance;
    }

    public DungeonSelectModel GetModel()
    {
        return model;
    }

    public List<UILevelInfo> GetStageLevels(int index)
    {
        return model.stage[index].levels;
    }
}