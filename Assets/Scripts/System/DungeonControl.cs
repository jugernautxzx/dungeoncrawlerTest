using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DungeonControl
{
    DungeonInventory dungeonInventory;
    List<Button> activeRoom = new List<Button>();

    float CurrentScale = 1f;

    public DungeonControl()
    {
        dungeonInventory = new DungeonInventory();
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
        for (int coridor = 0; coridor < DungeonModel.indexCoridor; coridor++)
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
        foreach(Button dungeon in activeRoom)
        {
            dungeon.GetComponent<Animator>().SetBool("Animate", false);
            dungeon.interactable = false;
        }
        activeRoom.Clear();

        for (int room = 0; room <= DungeonGenerator.info.allRoom; room++)
        {
            if (north)
            {
                if (PlayerPosition.y + 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y
                && PlayerPosition.x == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x)//North
                {
                    SetRoomToActive(DungeonRoom, room);
                }
            }

            if (east)
            {
                if (PlayerPosition.x + 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //East
                {
                    SetRoomToActive(DungeonRoom, room);
                }
            }
            if (south)
            {
                if (PlayerPosition.x == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y - 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //South
                {
                    SetRoomToActive(DungeonRoom, room);
                }
            }
            if (west)
            {
                if (PlayerPosition.x - 150 == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.x
                && PlayerPosition.y == DungeonRoom[room].GetComponent<RectTransform>().offsetMin.y) //West
                {
                    SetRoomToActive(DungeonRoom, room);
                }
            }
        }
    }

    public void SetRoomToActive(Button[] DungeonRoom,int room)
    {
        DungeonRoom[room].interactable = true;
        DungeonRoom[room].gameObject.SetActive(true);
        activeRoom.Add(DungeonRoom[room]);
        DungeonRoom[room].GetComponent<Animator>().SetBool("Animate", true);
    }

    public void CenterOnClick(Vector2 PlayerPosition, ScrollRect ScrollPanel)
    {
        float PositionRoomY;
        float PositionRoomX;
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;
        float NormalizeRoomPositionY = 0.5f;
        float NormalizeRoomPositionX = 0.5f;

        if (DungeonModel.maxPanelTopBottom >= 0)
        {
            PositionPerRoomTopBottom = 0.5f / ((DungeonModel.maxPanelTopBottom - 200) / 150f);
        }
        else
        {
            PositionPerRoomTopBottom = 0.5f / ((DungeonModel.maxPanelTopBottom + 200) / 150f);
        }

        if (DungeonModel.maxPanelRightLeft >= 0)
        {
            PositionPerRoomRightLeft = 0.5f / ((DungeonModel.maxPanelRightLeft - 200) / 150f);
        }
        else
        {
            PositionPerRoomRightLeft = 0.5f / ((DungeonModel.maxPanelRightLeft + 200) / 150f);
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

        if (DungeonModel.verticalNormalPosition > 0.5f)
        {
            if (NormalizeRoomPositionY > DungeonModel.verticalNormalPosition)
            {
                ScrollPanel.verticalNormalizedPosition = DungeonModel.verticalNormalPosition;
            }
        }
        else if (DungeonModel.verticalNormalPosition < 0.5f)
        {
            if (NormalizeRoomPositionY < DungeonModel.verticalNormalPosition)
            {
                ScrollPanel.verticalNormalizedPosition = DungeonModel.verticalNormalPosition;
            }
        }

        if (DungeonModel.horizontalNormalPosition > 0.5f)
        {
            if (NormalizeRoomPositionX > DungeonModel.horizontalNormalPosition)
            {
                ScrollPanel.horizontalNormalizedPosition = DungeonModel.horizontalNormalPosition;
            }
        }
        else if (DungeonModel.horizontalNormalPosition < 0.5f)
        {
            if (NormalizeRoomPositionX < DungeonModel.horizontalNormalPosition)
            {
                ScrollPanel.horizontalNormalizedPosition = DungeonModel.horizontalNormalPosition;
            }
        }
    }

    public void ClickRoomAction(Button[] DungeonRoom, GameObject TreasureActionPanel, GameObject TrapActionPanel, Text Log)
    {
        DungeonModel.currentDungeonRoom = DungeonRoom;
        DungeonModel.currentTreasureActionPanel = TreasureActionPanel;
        DungeonModel.currentTrapActionPanel = TrapActionPanel;
        DungeonModel.currentLog = Log;

        if (EventSystem.current.currentSelectedGameObject!=null)
        {
            DungeonModel.playerInRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        }

        if (DungeonRoom[DungeonModel.playerInRoom].tag.Contains("Enemy"))
        {
            EnemyEncounter();
            BattleEnemy(DungeonRoom,TreasureActionPanel,TrapActionPanel,Log);
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

        if (DungeonRoom[DungeonModel.playerInRoom].tag == "Treasure")
        {
            TreasureActionPanel.SetActive(true);
        }
        else
        {
            TreasureActionPanel.SetActive(false);

        }

        if (DungeonRoom[DungeonModel.playerInRoom].tag.Contains("Boss"))
        {
            BossEncounter();
            if (DungeonModel.battleWon == false)
            {
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
            }
            else
            {
                DungeonModel.battleWon = false;
            }
        }

        if (DungeonRoom[DungeonModel.playerInRoom].tag == "Untagged")
        {
            ClearRoomTag(DungeonRoom);
        }
        WriteLog(Log,DungeonRoom);

    }

    public void BattleEnemy(Button[] DungeonRoom, GameObject TreasureActionPanel, GameObject TrapActionPanel, Text Log)
    {
        if (DungeonModel.battleWon == false)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
        else if (DungeonRoom[DungeonModel.playerInRoom].tag.Contains("Treasure"))
        {
            DungeonRoom[DungeonModel.playerInRoom].tag = "Treasure";
            DungeonRoom[DungeonModel.playerInRoom].GetComponent<Image>().color = Color.yellow;
            ClickRoomAction(DungeonRoom, TreasureActionPanel, TrapActionPanel, Log);
            DungeonModel.battleWon = false;
        }
        else
        {
            ClearRoomTag(DungeonRoom);
            DungeonModel.battleWon = false;
        }
    }

    public void WriteLog(Text Log, Button[] DungeonRoom)
    {
        if (DungeonRoom[DungeonModel.playerInRoom].tag == "Treasure")
        {
            Log.text = "You see chest in this room";
        }
        else if (DungeonRoom[DungeonModel.playerInRoom].tag.Contains("Enemy"))
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
        string[] choosenEnemy=new string[4];
        int[] enemyLv = new int[4];
        int totalChance = 0;
        int totalEnemyWeight = 0;

        foreach (EnemyList enemyList in DungeonGenerator.info.enemy)
        {
            totalChance += enemyList.enemyMeetChance;
        }

        for (int enemy=0; totalEnemyWeight < 4;enemy++)
        {
            int currentChance = 0;
            int randEnemy = Random.Range(0,totalChance);
            foreach (EnemyList enemyList in DungeonGenerator.info.enemy)
            {
                currentChance += enemyList.enemyMeetChance;
                if (randEnemy<=currentChance)
                {
                    choosenEnemy[enemy] = enemyList.enemyId;
                    enemyLv[enemy] = enemyList.enemyLv;
                    totalEnemyWeight += enemyList.enemyWeight;

                    if (totalEnemyWeight>4)
                    {
                        totalEnemyWeight -= enemyList.enemyWeight;
                        enemy -= 1;
                    }
                    break;
                }
            }
           
        }
        SetEnemy(choosenEnemy,enemyLv);
    }

    public void SetEnemy(string[] choosenEnemy,int[] enemyLv)
    {
        DungeonModel.enemy1 = choosenEnemy[0];
        DungeonModel.lvEnemy1 = enemyLv[0];
        if (choosenEnemy[1] == null)
        {
            DungeonModel.enemy2 = null;
            DungeonModel.lvEnemy2 = 0;
        }
        else
        {
            DungeonModel.enemy2 = choosenEnemy[1];
            DungeonModel.lvEnemy2 = enemyLv[1];
        }

        if (choosenEnemy[2] == null)
        {
            DungeonModel.enemy3 = null;
            DungeonModel.lvEnemy3 = 0;
        }
        else
        {
            DungeonModel.enemy3 = choosenEnemy[2];
            DungeonModel.lvEnemy3 = enemyLv[2];
        }

        if (choosenEnemy[3] == null)
        {
            DungeonModel.enemy4 = null;
            DungeonModel.lvEnemy4 = 0;
        }
        else
        {
            DungeonModel.enemy4 = choosenEnemy[3];
            DungeonModel.lvEnemy4 = enemyLv[3];
        }

    }

    public void BossEncounter()
    {
        foreach (Boss boss in DungeonGenerator.info.boss)
        {
            DungeonModel.enemy1 = boss.bossId;
            DungeonModel.lvEnemy1 = boss.bossLv;
        }
        
    }

    public void ItemLoot(Text Log, Button[] DungeonRoom, GameObject TreasureActionPanel)
    {
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
            Log.text = "Unfortunetly the chest is empty";

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
            for (int itemIndex = 0; itemIndex < getItem.Count; itemIndex++)
            {
                //PlayerSession.GetProfile().AddItem(getItem[itemIndex], getAmount[itemIndex]);

                getLootControl(getItem[itemIndex],getAmount[itemIndex], ItemManager.GetInstance().GetItem(getItem[itemIndex]).item);
                getItemText += "You get " + ItemManager.GetInstance().GetItem(getItem[itemIndex]).name + " x" + getAmount[itemIndex] + "\n";
            }
            Log.text = getItemText;
        }
        ClearRoomTag(DungeonRoom);
        TreasureActionPanel.SetActive(false);
    }

    public void EquipmentLoot(Text Log, Button[] DungeonRoom, GameObject TreasureActionPanel, Text itemText)
    {
        int totalChance = 0;
        
        int getEquipment = Random.Range(DungeonGenerator.info.minGetEq, DungeonGenerator.info.maxGetEq + 1);
        foreach (EquipList equip in DungeonGenerator.info.equip)
        {
            totalChance += equip.chance;
        }

        if (getEquipment == 0)
        {
            Log.text = "Unfortunetly the chest is empty";

        }
        else
        {
            for (int equipmentCount = 1; equipmentCount <= getEquipment; equipmentCount++)
            {
                int currentChance = 0;
                int randomEquipmentLv = Random.Range(DungeonGenerator.info.minEqLv,DungeonGenerator.info.maxEqLv+1);
                int randomChance = Random.Range(0, totalChance);
                foreach (EquipList equip in DungeonGenerator.info.equip)
                {
                    currentChance += equip.chance;
                    if (randomChance <= currentChance)
                    {
                        Log.text = "You got " + equip.tier + " Lv" + randomEquipmentLv;
                        break;
                    }
                }
            }
        }
        ClearRoomTag(DungeonRoom);
        TreasureActionPanel.SetActive(false);

    }

    public void getLootControl(string itemId, int amount, ItemType type)
    {
        dungeonInventory.getLoot(itemId,amount,type);
    }

    public void ClearRoomTag(Button[] DungeonRoom)
    {
        DungeonRoom[DungeonModel.playerInRoom].GetComponent<Image>().color = Color.green;
        DungeonRoom[DungeonModel.playerInRoom].tag = "ClearRoom";
    }

    public void DungeonWin()
    {
        dungeonInventory.WinDungeonLoot();
    }

    public void DropItemControl(string itemId)
    {
        dungeonInventory.DropAction(itemId);
    }

    public void DropAllItemControl(string itemId)
    {
        dungeonInventory.DropAllAction(itemId);
    }

}
