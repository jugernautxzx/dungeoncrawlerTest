using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DungeonControl
{
    DungeonModel dungeonModel;
    BattleManager battleManager;
    
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
                DungeonCoridor[coridor].gameObject.SetActive(true);
            }
            if (PlayerPosition.x + 65 == DungeonCoridor[coridor].offsetMin.x
                && PlayerPosition.y + 45 == DungeonCoridor[coridor].offsetMax.y) //East
            {
                east = true;
                DungeonCoridor[coridor].gameObject.SetActive(true);
            }
            if (PlayerPosition.x + 25 == DungeonCoridor[coridor].offsetMin.x
               && PlayerPosition.y - 85 == DungeonCoridor[coridor].offsetMax.y) //South
            {
                south = true;
                DungeonCoridor[coridor].gameObject.SetActive(true);
            }
            if (PlayerPosition.x - 85 == DungeonCoridor[coridor].offsetMin.x
               && PlayerPosition.y + 45 == DungeonCoridor[coridor].offsetMax.y) //West
            {
                west = true;
                DungeonCoridor[coridor].gameObject.SetActive(true);
            }

        }

        //Check Room
        for (int room = 0; room <= DungeonGenerator.info.allRoom; room++)
        {
            DungeonRoom[room].interactable = false;
            if (north)
            {
                if (PlayerPosition.y + 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y
                && PlayerPosition.x == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x)//North
                {
                    DungeonRoom[room].interactable = true;
                    DungeonRoom[room].gameObject.SetActive(true);
                }
            }

            if (east)
            {
                if (PlayerPosition.x + 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //East
                {
                    DungeonRoom[room].interactable = true;
                    DungeonRoom[room].gameObject.SetActive(true);
                }
            }
            if (south)
            {
                if (PlayerPosition.x == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y - 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //South
                {
                    DungeonRoom[room].interactable = true;
                    DungeonRoom[room].gameObject.SetActive(true);
                }
            }
            if (west)
            {
                if (PlayerPosition.x - 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //West
                {
                    DungeonRoom[room].interactable = true;
                    DungeonRoom[room].gameObject.SetActive(true);
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

    public void ClickRoomAction(Button[] DungeonRoom, GameObject TreasureActionPanel, GameObject TrapActionPanel, Text Log)
    {
        DungeonModel.currentDungeonRoom = DungeonRoom;
        DungeonModel.currentTreasureActionPanel = TreasureActionPanel;
        DungeonModel.currentTrapActionPanel = TrapActionPanel;
        DungeonModel.CurrentLog = Log;

        if (EventSystem.current.currentSelectedGameObject!=null)
        {
            DungeonModel.PlayerInRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        }

        if (DungeonRoom[DungeonModel.PlayerInRoom].tag.Contains("Enemy"))
        {
            EnemyEncounter();
            if(DungeonModel.battleWon==false)
            {
                //SceneManager.LoadScene(2, LoadSceneMode.Additive);
            }
            else if (DungeonRoom[DungeonModel.PlayerInRoom].tag.Contains("Treasure"))
            {
                DungeonRoom[DungeonModel.PlayerInRoom].tag = "Treasure";
                DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.yellow;
                ClickRoomAction(DungeonRoom, TreasureActionPanel, TrapActionPanel,Log);
                DungeonModel.battleWon = false;
            }
            else
            {
                DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.green;
                DungeonRoom[DungeonModel.PlayerInRoom].tag = "ClearRoom";
                DungeonModel.battleWon = false;
            }
            //return;

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

        if (DungeonRoom[DungeonModel.PlayerInRoom].tag == "Treasure")
        {
            TreasureActionPanel.SetActive(true);
        }
        else
        {
            TreasureActionPanel.SetActive(false);

        }

        if (DungeonRoom[DungeonModel.PlayerInRoom].tag.Contains("Boss"))
        {
            //do something
            DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.green;
            DungeonRoom[DungeonModel.PlayerInRoom].tag = "ClearRoom";
        }

        if (DungeonRoom[DungeonModel.PlayerInRoom].tag == "Untagged")
        {
            DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.green;
            DungeonRoom[DungeonModel.PlayerInRoom].tag = "ClearRoom";
        }
        WriteLog(Log,DungeonRoom);

    }

    public void WriteLog(Text Log, Button[] DungeonRoom)
    {
        if (DungeonRoom[DungeonModel.PlayerInRoom].tag == "Treasure")
        {
            Log.text = "You see chest in this room";
        }
        else if (DungeonRoom[DungeonModel.PlayerInRoom].tag.Contains("Enemy"))
        {
            Log.text = "Encountered enemy";
        }
        else
        {
            Log.text = " ";
        }
    }

    public void EnemyEncounter()
    {
        int[] randEnemy=new int[4];

        for (int enemy=0;enemy<4;enemy++)
        {
            randEnemy[enemy] = Random.Range(0,DungeonGenerator.info.enemy.Count);
           
        }
        DungeonModel.enemy1 = DungeonGenerator.info.enemy[randEnemy[0]].enemyId;
        DungeonModel.lvEnemy1 = DungeonGenerator.info.enemy[randEnemy[0]].enemyLv;
        DungeonModel.enemy2 = DungeonGenerator.info.enemy[randEnemy[1]].enemyId;
        DungeonModel.lvEnemy2 = DungeonGenerator.info.enemy[randEnemy[1]].enemyLv;
        DungeonModel.enemy3 = DungeonGenerator.info.enemy[randEnemy[2]].enemyId;
        DungeonModel.lvEnemy3 = DungeonGenerator.info.enemy[randEnemy[2]].enemyLv;
        DungeonModel.enemy4 = DungeonGenerator.info.enemy[randEnemy[3]].enemyId;
        DungeonModel.lvEnemy4 = DungeonGenerator.info.enemy[randEnemy[3]].enemyLv;

    }

    public void ItemLoot(Text Log, Button[] DungeonRoom, GameObject TreasureActionPanel)
    {
        //do something
        int totalChance = 0;
        string getItemText = "";
        int randomAmount = 0;
        List<string> getItem = new List<string>();
        List<int> getAmount = new List<int>();

        int getTreasure = Random.Range(DungeonGenerator.info.minGet, DungeonGenerator.info.maxGet + 1);
        foreach (ItemList item in DungeonGenerator.info.item)
        {
            totalChance += item.chance;
        }

        if (getTreasure == 0)
        {
            Log.text = "Unfotunetly the chest is empty";

        }
        else
        {
            for (int treasureCount = 1; treasureCount <= getTreasure; treasureCount++)
            {
                int currentChance = 0;
                int randomChance = Random.Range(0, totalChance);
                foreach (ItemList item in DungeonGenerator.info.item)
                {
                    currentChance += item.chance;
                    if (randomChance <= currentChance)
                    {
                        randomAmount = Random.Range(item.amountMin, item.amountMax + 1);
                        
                        if (!getItem.Contains(item.itemId))
                        {
                            getItem.Add(item.itemId);
                            getAmount.Add(randomAmount);
                        }
                        else
                        {
                            int itemIndex = getItem.FindIndex(x=>x==item.itemId);
                            getAmount[itemIndex] += randomAmount;
                        }
                        break;
                    }
                }
                
            }
            Debug.Log(getItem.Count);
            for (int itemIndex = 0; itemIndex < getItem.Count; itemIndex++)
            {
                getItemText += "You get " + getItem[itemIndex] + " x" + getAmount[itemIndex] + "\n";
            }
            Log.text = getItemText;
        }
        DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.green;
        DungeonRoom[DungeonModel.PlayerInRoom].tag = "ClearRoom";
        TreasureActionPanel.SetActive(false);
    }
}
