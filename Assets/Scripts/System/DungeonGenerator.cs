using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour {

    public Text dungeonName;
    public Button Room;
    public Text Log;
    public RectTransform Panel;
    public ScrollRect ScrollPanel;
    public RectTransform CoridorX;
    public RectTransform CoridorY;
    public GameObject TreasureActionPanel;
    public GameObject TrapActionPanel;
    public Button ActionButton;
    public Text completeDungeon;
    public Button surrender;
    public Button win;
    public Text treasureItem;
    public RectTransform treasureContent;
    public Button consumableItem;
    public RectTransform consumableContent;
    public RectTransform treasure;
    public Button treasureTab;
    public Button consumableTab;
    public RectTransform consumable;
    public RectTransform inventoryContent;
    public static DungeonControl dungeonControl;

    int randPos;
    float panelTop = 0;
    float panelLeft = 0;
    float panelRight = 0;
    float panelBottom = 0;

    public Button[] DungeonRoom = new Button[200];
    public RectTransform[] DungeonCoridor = new RectTransform[200];
    public Button[] TreasureAction = new Button[5];
    public Button[] TrapAction = new Button[5];
    public static DungeonInfo info;

    void Start () {
        GenerateDungeon();
        StartInitialize();
        GenerateActionButton();
        SetInventoryObject();
    }

    void Update()
    {
        CalculateMaxScroll();
        dungeonControl.Zoom(Panel,Input.GetAxis("Mouse ScrollWheel"));
    }

    public DungeonGenerator()
    { 
        dungeonControl = new DungeonControl();
    }

    public void GenerateDungeon()
    {
        info = DungeonManager.DungeonLoad("Stage1_1");
        bool RoomPosition = true;
        DungeonModel.indexCoridor = 0;

        SpawnEntrancePoint();
        float RoomPositionX = DungeonRoom[0].GetComponent<RectTransform>().offsetMin.x;
        float RoomPositionY = DungeonRoom[0].GetComponent<RectTransform>().offsetMin.y;

        for (int RoomIndex = 1; RoomIndex <= info.allRoom; RoomIndex++)
        {
            DungeonRoom[RoomIndex] = Instantiate(Room);
            DungeonRoom[RoomIndex].transform.SetParent(Panel.transform, false);
            DungeonRoom[RoomIndex].name = RoomIndex + "";
            DungeonRoom[RoomIndex].onClick.AddListener(PlayerControl);

            do
            {
                randPos = Random.Range(1, 5);
                randposition(RoomIndex, RoomPositionX, RoomPositionY);

                RoomPosition = CheckRoom(RoomIndex, RoomPosition);

                SpawnCoridor(DungeonModel.indexCoridor, RoomPositionX, RoomPositionY);

                for (int K = 0; K < DungeonModel.indexCoridor; K++) //Check Coridor
                {

                    if (DungeonCoridor[K].offsetMin == DungeonCoridor[DungeonModel.indexCoridor].offsetMin)
                    {
                        Destroy(DungeonCoridor[DungeonModel.indexCoridor].gameObject);
                        DungeonModel.indexCoridor -= 1;
                        break;
                    }

                }

                RoomPositionX = DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin.x;
                RoomPositionY = DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin.y;
                DungeonCoridor[DungeonModel.indexCoridor].gameObject.SetActive(false);
                DungeonModel.indexCoridor += 1;

            } while (!RoomPosition);

            DungeonRoom[RoomIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            DungeonRoom[RoomIndex].gameObject.SetActive(false);

        }
        SizePanel();
        RandomRoomTag();
        EventSystem.current.SetSelectedGameObject(DungeonRoom[0].gameObject);
        EventSystem.current.currentSelectedGameObject.tag = "Entrance";
        DungeonRoom[0].GetComponent<Image>().color = Color.cyan;
        surrender.onClick.AddListener(Surrender);
        win.onClick.AddListener(WinUI);
        PlayerControl();
    }

    public void SpawnEntrancePoint()
    {
        DungeonRoom[0] = Instantiate(Room);                             //Set Entrance first
        DungeonRoom[0].transform.SetParent(Panel.transform, false);
        DungeonRoom[0].name = "0";
        DungeonRoom[0].onClick.AddListener(PlayerControl);
        DungeonRoom[0].GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        DungeonRoom[0].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
    }

    public void randposition(int i, float RoomPositionX, float RoomPositionY)
    {

        switch (randPos)
        {
            case 1:
                //North
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX, RoomPositionY + 150);
                break;
            case 2:
                //East
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX + 150, RoomPositionY);
                break;
            case 3:
                //South
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX, RoomPositionY - 150);
                break;
            case 4:
                //West
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX - 150, RoomPositionY);
                break;
        }

    }

    public bool CheckRoom(int RoomIndex, bool RoomPosition)
    {
        for (int PrevRoomIndex = 1; PrevRoomIndex < RoomIndex; PrevRoomIndex++) //Check Room
        {

            if (DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin == DungeonRoom[PrevRoomIndex].GetComponent<RectTransform>().offsetMin
                || DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin == new Vector2(0, 0))
            {
                RoomPosition = false;
                break;
            }
            else
            {
                RoomPosition = true;
            }
        }
        return RoomPosition;
    }

    public void SpawnCoridor(int IndexCoridor, float RoomPositionX, float RoomPositionY)
    {
        switch (randPos)
        {
            case 1:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorY);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX + 25, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY + 65);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(180, 16);
                break;

            case 2:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorX);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX + 65, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY + 45);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(180, 16);
                break;
            case 3:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorY);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX + 25, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY - 85);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(180, 16);
                break;

            case 4:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorX);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX - 85, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY + 45);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(180, 16);
                break;
        }
    }

    public void SizePanel()
    {

        for (int j = 0; j <= info.allRoom; j++)
        {
            if (DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y > panelTop)//Top
            {
                panelTop = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y;
            }

            if (DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x > panelRight) //Right
            {
                panelRight = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x;
            }

            if (DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y < panelBottom)//Bottom
            {
                panelBottom = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y;
            }

            if (DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x < panelLeft)//Left
            {
                panelLeft = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x;
            }

        }

        //Balance between Top Bottom, Right Left
        float MaxPanelTop = panelTop;
        float MaxPanelBottom = panelBottom;
        float MaxPanelRight = panelRight;
        float MaxPanelLeft = panelLeft;

        if (panelTop > Mathf.Abs(panelBottom))
        {
            DungeonModel.maxPanelTopBottom = panelTop;
            MaxPanelBottom = -panelTop;
        }
        else
        {
            DungeonModel.maxPanelTopBottom = panelBottom;
            MaxPanelTop = Mathf.Abs(panelBottom);
        }

        if (panelRight > Mathf.Abs(panelLeft))
        {
            DungeonModel.maxPanelRightLeft = panelRight;
            MaxPanelLeft = -panelRight;
        }
        else
        {
            DungeonModel.maxPanelRightLeft = panelLeft;
            MaxPanelRight = Mathf.Abs(panelLeft);
        }

        Panel.offsetMin = new Vector2(MaxPanelLeft + 200, MaxPanelBottom + 200);//Left Bottom
        Panel.offsetMax = new Vector2(MaxPanelRight - 200, MaxPanelTop - 200); //Right Top
    }

    public void CalculateMaxScroll()
    {
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;

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

        if (panelTop != Mathf.Abs(panelBottom))
        {
            if (PositionPerRoomTopBottom >= 0)
            {
                DungeonModel.verticalNormalPosition = 0.5f - (PositionPerRoomTopBottom * (Mathf.Abs(panelBottom + 200) / 150f));

                if (ScrollPanel.verticalNormalizedPosition <= DungeonModel.verticalNormalPosition)
                {
                    ScrollPanel.verticalNormalizedPosition = Mathf.Clamp01(DungeonModel.verticalNormalPosition);
                }
            }
            else
            {
                DungeonModel.verticalNormalPosition = 0.5f - (PositionPerRoomTopBottom * (Mathf.Abs(panelTop - 200) / 150f));

                if (ScrollPanel.verticalNormalizedPosition >= DungeonModel.verticalNormalPosition)
                { 
                    ScrollPanel.verticalNormalizedPosition = Mathf.Clamp01(DungeonModel.verticalNormalPosition);
                }
            }
        }

        if (panelRight != Mathf.Abs(panelLeft))
        {
            if (PositionPerRoomRightLeft >= 0)
            {
                DungeonModel.horizontalNormalPosition = 0.5f - (PositionPerRoomRightLeft * (Mathf.Abs(panelLeft + 200) / 150f));

                if (ScrollPanel.horizontalNormalizedPosition <= DungeonModel.horizontalNormalPosition)
                {
                    ScrollPanel.horizontalNormalizedPosition = Mathf.Clamp01(DungeonModel.horizontalNormalPosition);
                }
            }
            else
            {
                DungeonModel.horizontalNormalPosition = 0.5f - (PositionPerRoomRightLeft * (Mathf.Abs(panelRight - 200) / 150f));

                if (ScrollPanel.horizontalNormalizedPosition >= DungeonModel.horizontalNormalPosition)
                {
                    ScrollPanel.horizontalNormalizedPosition = Mathf.Clamp01(DungeonModel.horizontalNormalPosition);
                }
            }
        }
    }

    public void RandomRoomTag()
    {
        BossRoom();
        RandomEnemyRoom();
        RandomTrapRoom();
        RandomTreasureRoom();

    }

    public void BossRoom()
    {
        Vector2 mostFarRoom=new Vector2(0,0);
        int AddBoss = 0;

        for (int boss = 0; boss <= info.allRoom; boss++)
        {
            if (Mathf.Abs(DungeonRoom[boss].GetComponent<RectTransform>().offsetMin.y)>mostFarRoom.y
                && Mathf.Abs(DungeonRoom[boss].GetComponent<RectTransform>().offsetMin.x)>mostFarRoom.x)
            {
                mostFarRoom.y = DungeonRoom[boss].GetComponent<RectTransform>().offsetMin.y;
                mostFarRoom.x = DungeonRoom[boss].GetComponent<RectTransform>().offsetMin.x;
                AddBoss = boss;
            }
        }
        DungeonRoom[AddBoss].tag = "Boss";
        DungeonRoom[AddBoss].GetComponent<Image>().color = Color.red;

    }

    public void RandomEnemyRoom()
    {
        int AddEnemy = 0;
        List<int> EnemyRoom;

        EnemyRoom = new List<int>();
        for (int Enemy = 0; Enemy < info.maxEnemy; Enemy++)
        {
            AddEnemy = Random.Range(1, info.allRoom+1);
            while (EnemyRoom.Contains(AddEnemy) || DungeonRoom[AddEnemy].tag == "Boss")
            {
                AddEnemy = Random.Range(1, info.allRoom+1);
            }
            EnemyRoom.Add(AddEnemy);
            DungeonRoom[AddEnemy].tag = "Enemy";
            DungeonRoom[AddEnemy].GetComponent<Image>().color = Color.blue;
        }
    }

    public void RandomTreasureRoom()
    {
        int AddTreasure = 0;
        List<int> TreasureRoom;

        TreasureRoom = new List<int>();
        for (int Treasure = 0; Treasure < info.maxTreasure; Treasure++)
        {
            AddTreasure = Random.Range(1, info.allRoom+1);
            while (TreasureRoom.Contains(AddTreasure) || DungeonRoom[AddTreasure].tag == "Boss")
            {
                AddTreasure = Random.Range(1, info.allRoom+1);
            }
            TreasureRoom.Add(AddTreasure);
            if (DungeonRoom[AddTreasure].tag != "Untagged")
            {
                DungeonRoom[AddTreasure].tag = DungeonRoom[AddTreasure].tag + "Treasure";
            }
            else
            {
                DungeonRoom[AddTreasure].tag = "Treasure";
            }

            if (DungeonRoom[AddTreasure].tag.Contains("Enemy"))
            {
                DungeonRoom[AddTreasure].GetComponent<Image>().color = Color.blue;
            }
            else if (DungeonRoom[AddTreasure].tag.Contains("Trap"))
            {
                DungeonRoom[AddTreasure].GetComponent<Image>().color = Color.black;
            }
            else
            {
                DungeonRoom[AddTreasure].GetComponent<Image>().color = Color.yellow;
            }

        }
    }

    public void RandomTrapRoom()
    {
        int AddTrap = 0;
        List<int> TrapRoom;

        TrapRoom = new List<int>();
        for (int Treasure = 0; Treasure < info.maxTrap; Treasure++)
        {
            AddTrap = Random.Range(1, info.allRoom+1);
            while (TrapRoom.Contains(AddTrap) || DungeonRoom[AddTrap].tag == "Boss" || DungeonRoom[AddTrap].tag.Contains("Enemy"))
            {
                AddTrap = Random.Range(1, info.allRoom+1);
            }
            TrapRoom.Add(AddTrap);

            DungeonRoom[AddTrap].tag = "Trap";

            DungeonRoom[AddTrap].GetComponent<Image>().color = Color.black;
        }
    }

    public void PlayerControl()
    {
        int IndexRoom;
        IndexRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Vector2 PlayerPosition = DungeonRoom[IndexRoom].GetComponent<RectTransform>().offsetMin;
        dungeonControl.CenterOnClick(PlayerPosition,ScrollPanel);
        dungeonControl.ClickRoomAction(DungeonRoom,TreasureActionPanel,TrapActionPanel,Log);
        dungeonControl.WriteLog(Log, DungeonRoom);
        dungeonControl.CheckAccessedRoom(PlayerPosition,DungeonCoridor,DungeonRoom);

    }

    public void GenerateActionButton()
    {
        for (int Action = 0; Action < 1; Action++)
        {
            TreasureAction[Action] = Instantiate(ActionButton);
            TreasureAction[Action].transform.SetParent(TreasureActionPanel.transform, false);
        }

        TreasureAction[0].GetComponentInChildren<Text>().text = "Loot";
        TreasureAction[0].onClick.AddListener(LootAction);

        for (int Action = 0; Action < 1; Action++)
        {
            TrapAction[Action] = Instantiate(ActionButton);
            TrapAction[Action].transform.SetParent(TrapActionPanel.transform, false);
        }

        TrapAction[0].GetComponentInChildren<Text>().text = "Disarm";

    }

    public void LootAction()
    {
        int randomItem = Random.Range(0,100);

        /*if (randomItem<=info.chanceGetEq)
        {
            dungeonControl.EquipmentLoot(Log, DungeonRoom, TreasureActionPanel,item);
        }
        else
        {
            dungeonControl.ItemLoot(Log, DungeonRoom, TreasureActionPanel,item);
        }*/
        dungeonControl.ItemLoot(Log, DungeonRoom, TreasureActionPanel);
    }

    public void Surrender()
    {
        //PlayerSession.GetInstance().SaveSession();
        SceneManager.LoadScene(0);
    }

    public void WinUI()
    {
        completeDungeon.gameObject.SetActive(true);
        //Time.timeScale = 0;
        dungeonControl.DungeonWin();
    }

    public void SetInventoryObject() {
        DungeonModel.treasureItem = treasureItem;
        DungeonModel.treasureContent = treasureContent;
        DungeonModel.consumableContent = consumableContent;
        DungeonModel.consumableItem = consumableItem;
        DungeonModel.inventoryContent = inventoryContent;
        treasureTab.onClick.AddListener(ChangeToTreasureTab);
        consumableTab.onClick.AddListener(ChangeToConsumableTab);
    }

    public void StartInitialize()
    {
        dungeonName.text = info.name;
        completeDungeon.gameObject.SetActive(false);
        treasureTab.interactable = false;
        consumableTab.interactable = true;
    }

    public void ChangeToConsumableTab()
    {
        consumableTab.interactable = false;
        treasureTab.interactable = true;
        consumable.gameObject.SetActive(true);
        treasure.gameObject.SetActive(false);
    }

    public void ChangeToTreasureTab()
    {
        consumableTab.interactable = true;
        treasureTab.interactable = false;
        treasure.gameObject.SetActive(true);
        consumable.gameObject.SetActive(false);
    }

}

