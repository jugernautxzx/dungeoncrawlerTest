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
    int AllRoom = 100;

    Button[] DungeonRoom = new Button[200];
    RectTransform[] DungeonCoridor = new RectTransform[200];

    void Start () {
        StartCoroutine("GenerateDungeon");

    }

    IEnumerator GenerateDungeon()
    {
        bool RoomPosition = true;
  
        DungeonRoom[0] = Instantiate(Room);                             //Set Entrance first
        DungeonRoom[0].transform.SetParent(Panel.transform, false);
        DungeonRoom[0].name = "0";
        DungeonRoom[0].onClick.AddListener(PlayerPosition);
        DungeonRoom[0].GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        //DungeonRoom.offsetMin = new Vector2(DungeonRoom.offsetMin.y, 300);
        DungeonRoom[0].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
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
        //ScrollPanel.horizontalNormalizedPosition = 0.5f;
        //ScrollPanel.verticalNormalizedPosition = 0.5f;
        RandomRoomTag();
        EventSystem.current.SetSelectedGameObject(DungeonRoom[0].gameObject);
        EventSystem.current.currentSelectedGameObject.tag = "Entrance";
        DungeonRoom[0].GetComponent<Image>().color = Color.cyan;
        PlayerPosition();
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
        float PanelTop = 0;
        float PanelLeft = 0;
        float PanelRight = 0;
        float PanelBottom = 0;
        int PlusMaxTop = 0;
        int PlusMaxRight = 0;
        int PlusMaxLeft = 0;
        int PlusMaxBottom = 0;

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
        
        if (PanelTop>Mathf.Abs(PanelBottom))
        {
            PanelBottom = -PanelTop;
        }
        else
        {
            PanelTop = Mathf.Abs(PanelBottom);
        }

        if (PanelRight>Mathf.Abs(PanelLeft))
        {
            PanelLeft = -PanelRight;
        }
        else
        {
            PanelRight = Mathf.Abs(PanelLeft);
        }
        Panel.offsetMin = new Vector2(PanelLeft+400,PanelBottom+150 );//Left Bottom
        Panel.offsetMax = new Vector2(PanelRight-400, PanelTop-150); //Right Top
    }


    public void PlayerPosition ()
    {
        int IndexRoom;
        //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().offsetMin);
        IndexRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        Vector2 PlayerPosition= DungeonRoom[IndexRoom].GetComponent<RectTransform>().offsetMin ;
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

