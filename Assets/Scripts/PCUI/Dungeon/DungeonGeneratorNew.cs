using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorNew {

    List<DungeonModelNew> generatedRoom;
    List<DungeonCoridorModel> generatedCoridor;
    Vector2 roomPosition = new Vector2(0, 0);
    Vector2 coridorOffsetMax;
    Vector2 coridorOffsetMin;
    int randPos;

    public List<DungeonModelNew> SetDungeonRoom()
    {
        return generatedRoom;
    }

    public List<DungeonCoridorModel> SetDungeonCoridor()
    {
        return generatedCoridor;
    }

    public void GenerateDungeon(DungeonInfoNew dungeonInfo)
    {
        int coridorIndex = 0;

        generatedRoom = new List<DungeonModelNew>();
        generatedCoridor = new List<DungeonCoridorModel>();

        setEntrancePoint();
        for (int roomIndex=1;roomIndex<=dungeonInfo.allRoom;roomIndex++)
        {
            while (generatedRoom.Exists(t=>t.roomPosition==roomPosition))
            {
                randPos = Random.Range(1, 5);
                SetCoridor();
                SetRoomPosition();
                if(!generatedCoridor.Exists(t=>t.coridorOffsetMax==coridorOffsetMax && t.coridorOffsetMin==coridorOffsetMin))
                {
                    generatedCoridor.Add(new DungeonCoridorModel(coridorIndex++, randPos, coridorOffsetMax, coridorOffsetMin));
                }
                
            }
            generatedRoom.Add(new DungeonModelNew(roomIndex, roomPosition));

        }
    }

    public void setEntrancePoint()
    {
        generatedRoom.Add(new DungeonModelNew(0, roomPosition));
    }

    public void SetRoomPosition()
    {
        switch (randPos)
        {
            case 1:
                //North
                roomPosition = new Vector2(roomPosition.x, roomPosition.y + 150);
                break;
            case 2:
                //East
                roomPosition = new Vector2(roomPosition.x + 150, roomPosition.y);
                break;
            case 3:
                //South
                roomPosition = new Vector2(roomPosition.x, roomPosition.y - 150);
                break;
            case 4:
                //West
                roomPosition = new Vector2(roomPosition.x - 150, roomPosition.y);
                break;
        }
    }

    public void SetCoridor()
    {
        switch (randPos)
        {
            case 1:
                coridorOffsetMin = new Vector2(roomPosition.x + 25, 0);
                coridorOffsetMax = new Vector2(0, roomPosition.y + 65);
                break;

            case 2:
                coridorOffsetMin = new Vector2(roomPosition.x + 65, 0);
                coridorOffsetMax = new Vector2(0, roomPosition.y + 45);
                break;
            case 3:
                coridorOffsetMin = new Vector2(roomPosition.x + 25, 0);
                coridorOffsetMax = new Vector2(0, roomPosition.y - 85);
                break;

            case 4:
                coridorOffsetMin = new Vector2(roomPosition.x - 85, 0);
                coridorOffsetMax = new Vector2(0, roomPosition.y + 45);
                break;
        }
    }
}
