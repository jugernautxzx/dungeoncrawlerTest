using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonControlNew {

    public List<DungeonModelNew> dungeonModel;
    public List<DungeonCoridorModel> dungeonCoridorModel;
    public SetDungeonMap dungeonMap;

	public void InitializeDungeon(DungeonInfoNew dungeonInfo)
    {
        DungeonGeneratorNew dungeonGenerator = new DungeonGeneratorNew();
        dungeonGenerator.GenerateDungeon(dungeonInfo);
        dungeonModel = dungeonGenerator.SetDungeonRoom();
        dungeonCoridorModel = dungeonGenerator.SetDungeonCoridor();
        SaveSession();
    }

    public void SaveSession()
    {
        DungeonSession.GetSession().SetDungeonRoom(dungeonModel);
        DungeonSession.GetSession().SetDungeonCoridor(dungeonCoridorModel);
    }

    public List<DungeonModelNew> LoadDungeonRoomSession()
    {
        return DungeonSession.GetSession().GetDungeonRoom();
    }

    public List<DungeonCoridorModel> LoadDungeonCoridorSession()
    {
        return DungeonSession.GetSession().GetDungeonCoridor();
    }

    public Vector2 GetMapSize()
    {
        dungeonMap = new SetDungeonMap();
        return dungeonMap.SetSize(LoadDungeonRoomSession());
    }

    public Vector2 GetScrollMaxNormalizePosition()
    {
        return dungeonMap.CalculateMaxScrollMap();
    }

}
