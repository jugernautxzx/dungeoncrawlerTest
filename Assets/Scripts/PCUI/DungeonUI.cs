﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : MonoBehaviour {

    public Button room;
    public RectTransform dungeonMap;
    public RectTransform coridorY;
    public RectTransform coridorX;
    public ScrollRect dungeonMapScroll;

    DungeonControlNew dungeonControl;
    DungeonManagerNew dungeonInfo;

    void Start () {
        dungeonInfo=DungeonManagerNew.GetInstance("Stage1_1");
        dungeonControl = new DungeonControlNew();
        dungeonControl.InitializeDungeon(dungeonInfo.GetDungeonInfo());
        CreateNewRoom();
        CreateCoridor();
        SetMapSize();
    }

    void Update()
    {
        SetScrollMaxNormalizePosition();
    }

    public void CreateNewRoom()
    {
        List<DungeonModelNew> generatedDungeon = dungeonControl.LoadDungeonRoomSession();
        foreach (DungeonModelNew dungeonModel in generatedDungeon)
        {
            Button DungeonRoom = Instantiate(room,dungeonMap.transform,false);
            DungeonRoom.name = dungeonModel.roomId.ToString();
            DungeonRoom.GetComponent<RectTransform>().offsetMin = dungeonModel.roomPosition;
            DungeonRoom.GetComponent<RectTransform>().sizeDelta = new Vector2(70,70);
        }

    }

    public void CreateCoridor()
    {
        List<DungeonCoridorModel> generatedCoridor = dungeonControl.LoadDungeonCoridorSession();
        foreach (DungeonCoridorModel dungeonCoridorModel in generatedCoridor)
        {
            if (dungeonCoridorModel.coridorDirection==1||dungeonCoridorModel.coridorDirection==3)
            {
                RectTransform DungeonCoridor = Instantiate(coridorY,dungeonMap.transform,false);
                DungeonCoridor.offsetMin = dungeonCoridorModel.coridorOffsetMin;
                DungeonCoridor.offsetMax = dungeonCoridorModel.coridorOffsetMax;
                DungeonCoridor.sizeDelta = new Vector2(180, 16);
            }
            else
            {
                RectTransform DungeonCoridor = Instantiate(coridorX, dungeonMap.transform, false);
                DungeonCoridor.offsetMin = dungeonCoridorModel.coridorOffsetMin;
                DungeonCoridor.offsetMax = dungeonCoridorModel.coridorOffsetMax;
                DungeonCoridor.sizeDelta = new Vector2(180, 16);
            }
        }
    }

    public void SetMapSize()
    {
        float height = dungeonControl.GetMapSize().y+50;
        float width = dungeonControl.GetMapSize().x+50;

        dungeonMap.offsetMin= new Vector2(-width,-height); //Left Bottom
        dungeonMap.offsetMax= new Vector2(width, height); //Right Top
    }

    public void SetScrollMaxNormalizePosition()
    {
        float verticalNormalPosition;
        float horizontalNormalPosition;

        verticalNormalPosition = dungeonControl.GetScrollMaxNormalizePosition().y;
        horizontalNormalPosition = dungeonControl.GetScrollMaxNormalizePosition().x;

        if (verticalNormalPosition>=0.5)
        {
            dungeonMapScroll.verticalNormalizedPosition = 
                Mathf.Clamp(dungeonMapScroll.verticalNormalizedPosition,0,verticalNormalPosition);
        }
        else
        {
            dungeonMapScroll.verticalNormalizedPosition= 
                Mathf.Clamp(dungeonMapScroll.verticalNormalizedPosition, verticalNormalPosition, 1);
        }

        if (horizontalNormalPosition>=0.5)
        {
            
            dungeonMapScroll.horizontalNormalizedPosition= 
                Mathf.Clamp(dungeonMapScroll.horizontalNormalizedPosition, 0, horizontalNormalPosition);
            
        }
        else
        {
            dungeonMapScroll.horizontalNormalizedPosition =
                Mathf.Clamp(dungeonMapScroll.horizontalNormalizedPosition, horizontalNormalPosition, 1);
        }
    }
}
