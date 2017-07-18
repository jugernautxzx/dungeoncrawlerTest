using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DungeonControl
{
    DungeonModel dungeonModel;
    DungeonGenerator dungeonGenerator;
    
    float CurrentScale = 1f;

    public DungeonControl()
    {
        dungeonModel = new DungeonModel();
    }

    public void Zoom(RectTransform panel, float increment)
    {
        float MaxScale = 1f;
        float MinScale = 0.5f;

        CurrentScale += increment;
        if (CurrentScale >= MaxScale)
        {
            CurrentScale = MaxScale;
        }
        else if (CurrentScale <= MinScale)
        {
            CurrentScale = MinScale;
        }

        panel.localScale = new Vector2(CurrentScale, CurrentScale);

    }

    public void CheckAccessedRoom(Vector2 PlayerPosition, RectTransform[] DungeonCoridor, Button[] DungeonRoom)
    {
        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;

        //Check coridor
        for (int coridor = 0; coridor < DungeonModel.IndexCoridor; coridor++)
        {
            if (PlayerPosition.y + 65 == DungeonCoridor[coridor].offsetMax.y
                && PlayerPosition.x + 25 == DungeonCoridor[coridor].offsetMin.x) //North
            {
                north = true;
            }
            if (PlayerPosition.x + 65 == DungeonCoridor[coridor].offsetMin.x
                && PlayerPosition.y + 45 == DungeonCoridor[coridor].offsetMax.y) //East
            {
                east = true;
            }
            if (PlayerPosition.x + 25 == DungeonCoridor[coridor].offsetMin.x
               && PlayerPosition.y - 85 == DungeonCoridor[coridor].offsetMax.y) //South
            {
                south = true;
            }
            if (PlayerPosition.x - 85 == DungeonCoridor[coridor].offsetMin.x
               && PlayerPosition.y + 45 == DungeonCoridor[coridor].offsetMax.y) //West
            {
                west = true;
            }

        }

        //Check Room
        for (int room = 0; room <= dungeonModel.allRoom; room++)
        {
            DungeonRoom[room].interactable = false;
            if (north)
            {
                if (PlayerPosition.y + 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y
                && PlayerPosition.x == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x)//North
                {
                    DungeonRoom[room].interactable = true;
                }
            }

            if (east)
            {
                if (PlayerPosition.x + 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //East
                {
                    DungeonRoom[room].interactable = true;
                }
            }
            if (south)
            {
                if (PlayerPosition.x == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y - 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //South
                {
                    DungeonRoom[room].interactable = true;
                }
            }
            if (west)
            {
                if (PlayerPosition.x - 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //West
                {
                    DungeonRoom[room].interactable = true;
                }
            }
        }
    }

    public void CenterOnClick(Vector2 PlayerPosition, ScrollRect ScrollPanel)
    {
        float PositionRoomY;
        float PositionRoomX;
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;
        float NormalizeRoomPositionY = 0.5f;
        float NormalizeRoomPositionX = 0.5f;

        if (DungeonModel.MaxPanelTopBottom >= 0)
        {
            PositionPerRoomTopBottom = 0.5f / ((DungeonModel.MaxPanelTopBottom - 200) / 150f);
        }
        else
        {
            PositionPerRoomTopBottom = 0.5f / ((DungeonModel.MaxPanelTopBottom + 200) / 150f);
        }

        if (DungeonModel.MaxPanelRightLeft >= 0)
        {
            PositionPerRoomRightLeft = 0.5f / ((DungeonModel.MaxPanelRightLeft - 200) / 150f);
        }
        else
        {
            PositionPerRoomRightLeft = 0.5f / ((DungeonModel.MaxPanelRightLeft + 200) / 150f);
        }

        PositionRoomY = (PlayerPosition.y / 150) * PositionPerRoomTopBottom;
        PositionRoomX = (PlayerPosition.x / 150) * PositionPerRoomRightLeft;

        if (PlayerPosition.y >= 0)
        {
            NormalizeRoomPositionY = 0.5f + Mathf.Abs(PositionRoomY);
        }
        else if (PlayerPosition.y < 0)
        {
            NormalizeRoomPositionY = 0.5f - Mathf.Abs(PositionRoomY);
        }

        if (PlayerPosition.x >= 0)
        {
            NormalizeRoomPositionX = 0.5f + Mathf.Abs(PositionRoomX);
        }
        else if (PlayerPosition.x < 0)
        {
            NormalizeRoomPositionX = 0.5f - Mathf.Abs(PositionRoomX);
        }

        DoScrolling(NormalizeRoomPositionX, NormalizeRoomPositionY, ScrollPanel);

    }

    public void DoScrolling(float NormalizeRoomPositionX, float NormalizeRoomPositionY, ScrollRect ScrollPanel)
    {
        if (NormalizeRoomPositionY >= 1)
        {
            ScrollPanel.verticalNormalizedPosition = 1f;
        }
        else if (NormalizeRoomPositionY <= 0)
        {
            ScrollPanel.verticalNormalizedPosition = 0f;
        }
        else
        {
            ScrollPanel.verticalNormalizedPosition = NormalizeRoomPositionY;
        }

        if (NormalizeRoomPositionX >= 1)
        {
            ScrollPanel.horizontalNormalizedPosition = 1f;
        }
        else if (NormalizeRoomPositionX <= 0)
        {
            ScrollPanel.horizontalNormalizedPosition = 0f;
        }
        else
        {
            ScrollPanel.horizontalNormalizedPosition = NormalizeRoomPositionX;
        }

        if (DungeonModel.VerticalNormalPosition > 0.5f)
        {
            if (NormalizeRoomPositionY > DungeonModel.VerticalNormalPosition)
            {
                ScrollPanel.verticalNormalizedPosition = DungeonModel.VerticalNormalPosition;
            }
        }
        else if (DungeonModel.VerticalNormalPosition < 0.5f)
        {
            if (NormalizeRoomPositionY < DungeonModel.VerticalNormalPosition)
            {
                ScrollPanel.verticalNormalizedPosition = DungeonModel.VerticalNormalPosition;
            }
        }

        if (DungeonModel.HorizontalNormalPosition > 0.5f)
        {
            if (NormalizeRoomPositionX > DungeonModel.HorizontalNormalPosition)
            {
                ScrollPanel.horizontalNormalizedPosition = DungeonModel.HorizontalNormalPosition;
            }
        }
        else if (DungeonModel.HorizontalNormalPosition < 0.5f)
        {
            if (NormalizeRoomPositionX < DungeonModel.HorizontalNormalPosition)
            {
                ScrollPanel.horizontalNormalizedPosition = DungeonModel.HorizontalNormalPosition;
            }
        }
    }

    public void ClickRoomAction(Button[] DungeonRoom, GameObject TreasureActionPanel, GameObject TrapActionPanel)
    {
        DungeonModel.PlayerInRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        bool EnemyDefeated;

        if (EventSystem.current.currentSelectedGameObject.tag.Contains("Enemy"))
        {
            //do something
            EnemyDefeated = true;

            if (EventSystem.current.currentSelectedGameObject.tag.Contains("Treasure") && EnemyDefeated == true)
            {
                DungeonRoom[DungeonModel.PlayerInRoom].tag = "Treasure";
                DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.yellow;
                ClickRoomAction(DungeonRoom,TreasureActionPanel,TrapActionPanel);
            }
            if (!EventSystem.current.currentSelectedGameObject.tag.Contains("Treasure") && EnemyDefeated == true)
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
                EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
            }
        }

        /*if (EventSystem.current.currentSelectedGameObject.tag.Contains("Trap"))
        {
            //do something
            if (EventSystem.current.currentSelectedGameObject.tag.Contains("Treasure"))
            {
                TreasureActionPanel.SetActive(true);
            }
            else
            {
                TreasureActionPanel.SetActive(false);
            }
            if (!EventSystem.current.currentSelectedGameObject.tag.Contains("Treasure"))
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
                EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
            }
            
        }*/

        if (EventSystem.current.currentSelectedGameObject.tag == "Treasure")
        {
            TreasureActionPanel.SetActive(true);
        }
        else
        {
            TreasureActionPanel.SetActive(false);

        }

        if (EventSystem.current.currentSelectedGameObject.tag.Contains("Boss"))
        {
            //do something
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
            EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
        }

        if (EventSystem.current.currentSelectedGameObject.tag == "Untagged")
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
            EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
        }


    }
    public void WriteLog(Text Log)
    {
        if (EventSystem.current.currentSelectedGameObject.tag == "Treasure")
        {
            Log.text = "You see chest in this room";
        }
        else if (EventSystem.current.currentSelectedGameObject.tag.Contains("Enemy"))
        {
            Log.text = "Encountered enemy";
        }
        else
        {
            Log.text = " ";
        }
    }

}
