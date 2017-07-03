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
    int AllRoom = 20;

    Button[] DungeonRoom = new Button[100];
    RectTransform[] DungeonCoridor = new RectTransform[100];

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
        ScrollPanel.horizontalNormalizedPosition = 0.5f;
        ScrollPanel.verticalNormalizedPosition = 0.5f;
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

            if (DungeonRoom[RoomIndex].GetComponent<RectTransform>().offsetMin == DungeonRoom[PrevRoomIndex].GetComponent<RectTransform>().offsetMin)
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

        for (int j=1; j<= AllRoom; j++)
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
        if (PanelTop <= 150)
        {
            PlusMaxTop = 0;
            PanelTop = 0;
        }
        else PlusMaxTop = 150;

        if (PanelRight <= 150)
        {
            PlusMaxRight = 0;
            PanelRight = 0;
        }
        else PlusMaxRight = 150;

        if (PanelLeft >= -150)
        {
            PlusMaxLeft = 0;
            PanelLeft = 0;
        }
        else PlusMaxLeft = 150;

        if (PanelBottom >= -150)
        {
            PlusMaxBottom = 0;
            PanelBottom = 0;
        }
        else PlusMaxBottom = 150;
        
        Panel.offsetMin = new Vector2(-Mathf.Abs(PanelLeft)-PlusMaxLeft, -Mathf.Abs(PanelBottom)-PlusMaxBottom);//Left Bottom
        Panel.offsetMax = new Vector2(PanelRight+PlusMaxRight, PanelTop+PlusMaxTop); //Right Top
        
        
    }


    public void PlayerPosition ()
    {
        int IndexRoom;
        //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().offsetMin);
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            IndexRoom = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        }
        else
        {
            IndexRoom = 0;
        }
        Vector2 PlayerPosition= DungeonRoom[IndexRoom].GetComponent<RectTransform>().offsetMin ;
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

    }
}

