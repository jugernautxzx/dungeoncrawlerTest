using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DungeonGenerator : MonoBehaviour {

    public Button Room;
    public GameObject[] Rooms;
    public RectTransform Panel;
    public ScrollRect ScrollPanel;
    public RectTransform CoridorX;
    public RectTransform CoridorY;

    int test=0;
    int randpos;
    int IndexCoridor=0;
    int AllRoom = 20;     //Total Dungeon Room
    float PanelTop = 0;
    float PanelLeft = 0;
    float PanelRight = 0;
    float PanelBottom = 0;
    float MaxPanelTopBottom=0;
    float MaxPanelRightLeft = 0;
    float CurrentScale = 1f;

    Button[] DungeonRoom = new Button[200];
    RectTransform[] DungeonCoridor = new RectTransform[200];

    void Start () {
        StartCoroutine("GenerateDungeon");

    }

    void Update()
    {
        CalculateMaxScroll();
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    IEnumerator GenerateDungeon()
    {
        bool RoomPosition = true;

        SpawnEntrancePoint();
        float RoomPositionX = DungeonRoom[0].GetComponent<RectTransform>().offsetMin.x;
        float RoomPositionY = DungeonRoom[0].GetComponent<RectTransform>().offsetMin.y;

        for (int RoomIndex = 1; RoomIndex <= AllRoom; RoomIndex++)
        {
            DungeonRoom[RoomIndex] = Instantiate(Room);
            DungeonRoom[RoomIndex].transform.SetParent(Panel.transform, false);
            DungeonRoom[RoomIndex].name = RoomIndex+"";
            DungeonRoom[RoomIndex].onClick.AddListener(PlayerPosition);

            do
            {
                randpos = Random.Range(1, 5);
                randposition(RoomIndex, RoomPositionX, RoomPositionY);

                RoomPosition= CheckRoom(RoomIndex,RoomPosition);
                
                SpawnCoridor(IndexCoridor, RoomPositionX, RoomPositionY);

                for (int K=0;K<IndexCoridor;K++) //Check Coridor
                {
                    
                    if (DungeonCoridor[K].offsetMin==DungeonCoridor[IndexCoridor].offsetMin)
                    {
                        Destroy(DungeonCoridor[IndexCoridor].gameObject);
                        IndexCoridor -= 1;
                        break;
                    }
                    
                }
                
                RoomPositionX = DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin.x;
                RoomPositionY = DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin.y;
                IndexCoridor += 1;
                
            } while (!RoomPosition);
            
                DungeonRoom[RoomIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
                yield return new WaitForSeconds(.1f);

        }
        SizePanel();
        RandomRoomTag();
        EventSystem.current.SetSelectedGameObject(DungeonRoom[0].gameObject);
        EventSystem.current.currentSelectedGameObject.tag = "Entrance";
        DungeonRoom[0].GetComponent<Image>().color = Color.cyan;
        PlayerPosition();
    }

    public void SpawnEntrancePoint()
    {
        DungeonRoom[0] = Instantiate(Room);                             //Set Entrance first
        DungeonRoom[0].transform.SetParent(Panel.transform, false);
        DungeonRoom[0].name = "0";
        DungeonRoom[0].onClick.AddListener(PlayerPosition);
        DungeonRoom[0].GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        DungeonRoom[0].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
    }

    public void randposition(int i,float RoomPositionX,float RoomPositionY)
    {
        
        switch (randpos)
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
                || DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin==new Vector2(0,0))
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
        switch (randpos)
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
        
        for (int j=0; j<= AllRoom; j++)
        {
            if (DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y > PanelTop)//Top
            {
                PanelTop = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y; 
            }

            if (DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x > PanelRight) //Right
            {
                PanelRight = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x;
            }

            if(DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y < PanelBottom)//Bottom
            {
                PanelBottom = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.y;
            }

            if(DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x < PanelLeft)//Left
            {
                PanelLeft = DungeonRoom[j].GetComponent<RectTransform>().offsetMin.x;
            }

        }

        float MaxPanelTop = PanelTop;
        float MaxPanelBottom = PanelBottom;
        float MaxPanelRight = PanelRight;
        float MaxPanelLeft = PanelLeft;

        if (PanelTop>Mathf.Abs(PanelBottom))
        {
            MaxPanelTopBottom = PanelTop;
            MaxPanelBottom = -PanelTop;
        }
        else
        {
            MaxPanelTopBottom = PanelBottom;
            MaxPanelTop = Mathf.Abs(PanelBottom);
        }

        if (PanelRight>Mathf.Abs(PanelLeft))
        {
            MaxPanelRightLeft = PanelRight;
            MaxPanelLeft = -PanelRight;
        }
        else
        {
            MaxPanelRightLeft = PanelLeft;
            MaxPanelRight = Mathf.Abs(PanelLeft);
        }

        Panel.offsetMin = new Vector2(MaxPanelLeft+200,  MaxPanelBottom+200 );//Left Bottom
        Panel.offsetMax = new Vector2(MaxPanelRight-200, MaxPanelTop-200); //Right Top
    }


    public void PlayerPosition ()
    {
        int IndexRoom;
        IndexRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Vector2 PlayerPosition= DungeonRoom[IndexRoom].GetComponent<RectTransform>().offsetMin ;
        CenterOnClick(PlayerPosition);
        ClickRoomAction();
        CheckAccessedRoom(PlayerPosition);
    }

    public void CheckAccessedRoom (Vector2 PlayerPosition)
    {
        bool north = false;
        bool south = false;
        bool east = false;
        bool west = false;

        //Check coridor
        for (int coridor = 0; coridor < IndexCoridor; coridor++)
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
        for (int room = 0; room <= AllRoom; room++)
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

    public void RandomRoomTag ()
    {
        RandomBossRoom();
        RandomEnemyRoom();
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
            AddBoss = Random.Range(AllRoom/2, AllRoom);
            while (BossRoom.Contains(AddBoss))
            {
                AddBoss = Random.Range(1, AllRoom);
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
            AddEnemy = Random.Range(1, AllRoom);
            while (EnemyRoom.Contains(AddEnemy) || DungeonRoom[AddEnemy].tag=="Boss")
            {
                AddEnemy = Random.Range(1, AllRoom);
            }
            EnemyRoom.Add(AddEnemy);
            DungeonRoom[AddEnemy].tag ="Enemy";
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
            AddTreasure = Random.Range(1, AllRoom);
            while (TreasureRoom.Contains(AddTreasure)|| DungeonRoom[AddTreasure].tag == "Boss")
            {
                AddTreasure = Random.Range(1, AllRoom);
            }
            TreasureRoom.Add(AddTreasure);
            if(DungeonRoom[AddTreasure].tag!="Untagged")
            {
                DungeonRoom[AddTreasure].tag = DungeonRoom[AddTreasure].tag + "Treasure";
            }
            else
            {
                DungeonRoom[AddTreasure].tag ="Treasure";
            }
            DungeonRoom[AddTreasure].GetComponent<Image>().color = Color.yellow;
        }
    }

    public void CalculateMaxScroll()
    {
        float VerticalNormalPosition;
        float HorizontalNormalPosition;
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;

        if (MaxPanelTopBottom>=0)
        {
            PositionPerRoomTopBottom = 0.5f / ((MaxPanelTopBottom - 200) / 150f);
        }
        else
        {
            PositionPerRoomTopBottom = 0.5f / ((MaxPanelTopBottom + 200) / 150f);
        }

        if (MaxPanelRightLeft>=0)
        {
            PositionPerRoomRightLeft = 0.5f / ((MaxPanelRightLeft - 200) / 150f);
        }
        else
        {
            PositionPerRoomRightLeft = 0.5f / ((MaxPanelRightLeft + 200) / 150f);
        }

        if (PanelTop!=Mathf.Abs(PanelBottom))
        {
            if (PositionPerRoomTopBottom >= 0)
            {
                VerticalNormalPosition = 0.5f - (PositionPerRoomTopBottom * (Mathf.Abs(PanelBottom+200) / 150f));

                if (ScrollPanel.verticalNormalizedPosition < VerticalNormalPosition)
                {
                    ScrollPanel.verticalNormalizedPosition = VerticalNormalPosition;
                }
            }
            else
            {
                VerticalNormalPosition = 0.5f - (PositionPerRoomTopBottom * (Mathf.Abs(PanelTop+200) / 150f));

                if (ScrollPanel.verticalNormalizedPosition > VerticalNormalPosition)
                {
                    ScrollPanel.verticalNormalizedPosition = VerticalNormalPosition;
                }
            }
        }

        if (PanelRight != Mathf.Abs(PanelLeft))
        {
            if (PositionPerRoomRightLeft >= 0)
            {
                HorizontalNormalPosition = 0.5f - (PositionPerRoomRightLeft * (Mathf.Abs(PanelLeft+200) / 150f));

                if (ScrollPanel.horizontalNormalizedPosition < HorizontalNormalPosition)
                {
                    ScrollPanel.horizontalNormalizedPosition = HorizontalNormalPosition;
                }
            }
            else
            {
                HorizontalNormalPosition = 0.5f - (PositionPerRoomRightLeft * (Mathf.Abs(PanelRight+200) / 150f));

                if (ScrollPanel.horizontalNormalizedPosition > HorizontalNormalPosition)
                {
                    ScrollPanel.horizontalNormalizedPosition = HorizontalNormalPosition;
                }
            }
        }
    }

    public void Zoom(float increment)
    {
        float MaxScale = 1f;
        float MinScale = 0.5f;

        CurrentScale += increment;
        if (CurrentScale>=MaxScale)
        {
            CurrentScale = MaxScale;
        }else if (CurrentScale<=MinScale)
        {
            CurrentScale = MinScale;
        }

        Panel.localScale = new Vector2(CurrentScale,CurrentScale);

    }

    public void CenterOnClick(Vector2 PlayerPosition)
    {
        float PositionRoomY;
        float PositionRoomX;
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;
        float NormalizeRoomPositionY=0.5f;
        float NormalizeRoomPositionX=0.5f;

        if (MaxPanelTopBottom >= 0)
        {
            PositionPerRoomTopBottom = 0.5f / ((MaxPanelTopBottom - 200) / 150f);
        }
        else
        {
            PositionPerRoomTopBottom = 0.5f / ((MaxPanelTopBottom + 200) / 150f);
        }

        if (MaxPanelRightLeft >= 0)
        {
            PositionPerRoomRightLeft = 0.5f / ((MaxPanelRightLeft - 200) / 150f);
        }
        else
        {
            PositionPerRoomRightLeft = 0.5f / ((MaxPanelRightLeft + 200) / 150f);
        }

        PositionRoomY = (PlayerPosition.y / 150) * PositionPerRoomTopBottom;
        PositionRoomX = (PlayerPosition.x / 150) * PositionPerRoomRightLeft;

        if (PlayerPosition.y>=0)
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

        DoScrolling(NormalizeRoomPositionY,NormalizeRoomPositionX);
        
    }

    public void DoScrolling(float NormalizeRoomPositionY, float NormalizeRoomPositionX)
    {
        if (NormalizeRoomPositionY > 1)
        {
            ScrollPanel.verticalNormalizedPosition = 1f;
        }
        else if (NormalizeRoomPositionY < 0)
        {
            ScrollPanel.verticalNormalizedPosition = 0f;
        }
        else
        {
            ScrollPanel.verticalNormalizedPosition = NormalizeRoomPositionY;
        }

        if (NormalizeRoomPositionX > 1)
        {
            ScrollPanel.horizontalNormalizedPosition = 1f;
        }
        else if (NormalizeRoomPositionX < 0)
        {
            ScrollPanel.horizontalNormalizedPosition = 0f;
        }
        else
        {
            ScrollPanel.horizontalNormalizedPosition = NormalizeRoomPositionX;
        }
    }

    public void ClickRoomAction()
    {

        if (EventSystem.current.currentSelectedGameObject.tag.Contains("Enemy"))
        {
            //do something

            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
            EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
        }

        if (EventSystem.current.currentSelectedGameObject.tag.Contains("Treasure"))
        {
            //do something
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
            EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
        }

        if (EventSystem.current.currentSelectedGameObject.tag.Contains("Boss"))
        {
            //do something
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
            EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
        }

        if(EventSystem.current.currentSelectedGameObject.tag=="Untagged")
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
            EventSystem.current.currentSelectedGameObject.tag = "ClearRoom";
        }


    }
}

