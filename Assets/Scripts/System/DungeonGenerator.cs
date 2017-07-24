using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour {

    public Text DungeonName;
    public Button Room;
    public Text Log;
    public RectTransform Panel;
    public ScrollRect ScrollPanel;
    public RectTransform CoridorX;
    public RectTransform CoridorY;
    public GameObject TreasureActionPanel;
    public GameObject TrapActionPanel;
    public Button ActionButton;
    DungeonControl dungeonControl;
    DungeonModel dungeonModel;
    DungeonManager dungeonManager;
    DungeonInfo dungeonInfo;

    int randPos;
    float panelTop = 0;
    float panelLeft = 0;
    float panelRight = 0;
    float panelBottom = 0;

    public Button[] DungeonRoom = new Button[200];
    public RectTransform[] DungeonCoridor = new RectTransform[200];
    public Button[] TreasureAction = new Button[5];
    public Button[] TrapAction = new Button[5];
    public static DungeonInfo Info;

    void Start () {
        dungeonManager = new DungeonManager();
        GenerateDungeon();
        GenerateActionButton();
    }

    void Update()
    {
        CalculateMaxScroll();
        dungeonControl.Zoom(Panel,Input.GetAxis("Mouse ScrollWheel"));
    }

    public DungeonGenerator()
    { 
        dungeonModel = new DungeonModel();
        dungeonControl = new DungeonControl();
    }

    public void GenerateDungeon()
    {
        Info = DungeonManager.DungeonLoad("Stage1_1");
        bool RoomPosition = true;
        DungeonModel.IndexCoridor = 0;
        DungeonName.text = Info.name;

        SpawnEntrancePoint();
        float RoomPositionX = DungeonRoom[0].GetComponent<RectTransform>().offsetMin.x;
        float RoomPositionY = DungeonRoom[0].GetComponent<RectTransform>().offsetMin.y;

        for (int RoomIndex = 1; RoomIndex <= Info.allRoom; RoomIndex++)
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

                SpawnCoridor(DungeonModel.IndexCoridor, RoomPositionX, RoomPositionY);

                for (int K = 0; K < DungeonModel.IndexCoridor; K++) //Check Coridor
                {

                    if (DungeonCoridor[K].offsetMin == DungeonCoridor[DungeonModel.IndexCoridor].offsetMin)
                    {
                        Destroy(DungeonCoridor[DungeonModel.IndexCoridor].gameObject);
                        DungeonModel.IndexCoridor -= 1;
                        break;
                    }

                }

                RoomPositionX = DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin.x;
                RoomPositionY = DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin.y;
                DungeonModel.IndexCoridor += 1;

            } while (!RoomPosition);

            DungeonRoom[RoomIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);

        }
        SizePanel();
        RandomRoomTag();
        EventSystem.current.SetSelectedGameObject(DungeonRoom[0].gameObject);
        EventSystem.current.currentSelectedGameObject.tag = "Entrance";
        DungeonRoom[0].GetComponent<Image>().color = Color.cyan;
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

        for (int j = 0; j <= Info.allRoom; j++)
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

        float MaxPanelTop = panelTop;
        float MaxPanelBottom = panelBottom;
        float MaxPanelRight = panelRight;
        float MaxPanelLeft = panelLeft;

        if (panelTop > Mathf.Abs(panelBottom))
        {
            DungeonModel.MaxPanelTopBottom = panelTop;
            MaxPanelBottom = -panelTop;
        }
        else
        {
            DungeonModel.MaxPanelTopBottom = panelBottom;
            MaxPanelTop = Mathf.Abs(panelBottom);
        }

        if (panelRight > Mathf.Abs(panelLeft))
        {
            DungeonModel.MaxPanelRightLeft = panelRight;
            MaxPanelLeft = -panelRight;
        }
        else
        {
            DungeonModel.MaxPanelRightLeft = panelLeft;
            MaxPanelRight = Mathf.Abs(panelLeft);
        }

        Panel.offsetMin = new Vector2(MaxPanelLeft + 200, MaxPanelBottom + 200);//Left Bottom
        Panel.offsetMax = new Vector2(MaxPanelRight - 200, MaxPanelTop - 200); //Right Top
    }

    public void CalculateMaxScroll()
    {
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;
        float lerptime = 0.5f;

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

        if (panelTop != Mathf.Abs(panelBottom))
        {
            if (PositionPerRoomTopBottom >= 0)
            {
                DungeonModel.VerticalNormalPosition = 0.5f - (PositionPerRoomTopBottom * (Mathf.Abs(panelBottom + 200) / 150f));

                if (ScrollPanel.verticalNormalizedPosition <= DungeonModel.VerticalNormalPosition)
                {
                    ScrollPanel.verticalNormalizedPosition = Mathf.Lerp(ScrollPanel.verticalNormalizedPosition, DungeonModel.VerticalNormalPosition, Time.deltaTime / lerptime);
                }
            }
            else
            {
                DungeonModel.VerticalNormalPosition = 0.5f - (PositionPerRoomTopBottom * (Mathf.Abs(panelTop - 200) / 150f));

                if (ScrollPanel.verticalNormalizedPosition >= DungeonModel.VerticalNormalPosition)
                { 
                    ScrollPanel.verticalNormalizedPosition = Mathf.Lerp(ScrollPanel.verticalNormalizedPosition, DungeonModel.VerticalNormalPosition, Time.deltaTime / lerptime);
                }
            }
        }

        if (panelRight != Mathf.Abs(panelLeft))
        {
            if (PositionPerRoomRightLeft >= 0)
            {
                DungeonModel.HorizontalNormalPosition = 0.5f - (PositionPerRoomRightLeft * (Mathf.Abs(panelLeft + 200) / 150f));

                if (ScrollPanel.horizontalNormalizedPosition <= DungeonModel.HorizontalNormalPosition)
                {
                    ScrollPanel.horizontalNormalizedPosition = Mathf.Lerp(ScrollPanel.horizontalNormalizedPosition, DungeonModel.HorizontalNormalPosition, Time.deltaTime / lerptime);
                }
            }
            else
            {
                DungeonModel.HorizontalNormalPosition = 0.5f - (PositionPerRoomRightLeft * (Mathf.Abs(panelRight - 200) / 150f));

                if (ScrollPanel.horizontalNormalizedPosition >= DungeonModel.HorizontalNormalPosition)
                {
                    ScrollPanel.horizontalNormalizedPosition = Mathf.Lerp(ScrollPanel.horizontalNormalizedPosition, DungeonModel.HorizontalNormalPosition, Time.deltaTime / lerptime);
                }
            }
        }
    }

    public void RandomRoomTag()
    {
        RandomBossRoom();
        RandomEnemyRoom();
        RandomTrapRoom();
        RandomTreasureRoom();

    }

    public void RandomBossRoom()
    {
        int MaxBoss = 1;
        int AddBoss = 0;
        List<int> BossRoom;

        BossRoom = new List<int>();
        for (int Boss = 0; Boss < MaxBoss; Boss++)
        {
            AddBoss = Random.Range(Info.allRoom / 2, Info.allRoom);
            while (BossRoom.Contains(AddBoss))
            {
                AddBoss = Random.Range(1, Info.allRoom);
            }
            BossRoom.Add(AddBoss);
            DungeonRoom[AddBoss].tag = "Boss";
            DungeonRoom[AddBoss].GetComponent<Image>().color = Color.red;
        }

    }

    public void RandomEnemyRoom()
    {
        int MaxEnemy = 5;
        int AddEnemy = 0;
        List<int> EnemyRoom;

        EnemyRoom = new List<int>();
        for (int Enemy = 0; Enemy < MaxEnemy; Enemy++)
        {
            AddEnemy = Random.Range(1, Info.allRoom);
            while (EnemyRoom.Contains(AddEnemy) || DungeonRoom[AddEnemy].tag == "Boss")
            {
                AddEnemy = Random.Range(1, Info.allRoom);
            }
            EnemyRoom.Add(AddEnemy);
            DungeonRoom[AddEnemy].tag = "Enemy";
            DungeonRoom[AddEnemy].GetComponent<Image>().color = Color.blue;
        }
    }

    public void RandomTreasureRoom()
    {
        int MaxTreasure = 5;
        int AddTreasure = 0;
        List<int> TreasureRoom;

        TreasureRoom = new List<int>();
        for (int Treasure = 0; Treasure < MaxTreasure; Treasure++)
        {
            AddTreasure = Random.Range(1, Info.allRoom);
            while (TreasureRoom.Contains(AddTreasure) || DungeonRoom[AddTreasure].tag == "Boss")
            {
                AddTreasure = Random.Range(1, Info.allRoom);
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
        int MaxTrap = 5;
        int AddTrap = 0;
        List<int> TrapRoom;

        TrapRoom = new List<int>();
        for (int Treasure = 0; Treasure < MaxTrap; Treasure++)
        {
            AddTrap = Random.Range(1, Info.allRoom);
            while (TrapRoom.Contains(AddTrap) || DungeonRoom[AddTrap].tag == "Boss" || DungeonRoom[AddTrap].tag.Contains("Enemy"))
            {
                AddTrap = Random.Range(1, Info.allRoom);
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
        dungeonControl.ClickRoomAction(DungeonRoom,TreasureActionPanel,TrapActionPanel);
        dungeonControl.WriteLog(Log);
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
        //do something
        Debug.Log("Looted");
        DungeonRoom[DungeonModel.PlayerInRoom].GetComponent<Image>().color = Color.green;
        DungeonRoom[DungeonModel.PlayerInRoom].tag = "ClearRoom";
        TreasureActionPanel.SetActive(false);
    }

}

