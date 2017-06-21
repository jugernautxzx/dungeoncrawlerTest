using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungeonGenerator : MonoBehaviour {

    public RectTransform Room;
    public RectTransform Panel;
    public ScrollRect ScrollPanel;
    public RectTransform CoridorX;
    public RectTransform CoridorY;

    int test=0;
    int randpos;
    int IndexCoridor=0;
    int AllRoom = 20;

    RectTransform[] DungeonRoom = new RectTransform[100];
    RectTransform[] DungeonCoridor = new RectTransform[100];

    void Start () {
        StartCoroutine("GenerateDungeon");

    }

    IEnumerator GenerateDungeon()
    {
        bool RoomPosition = true;
        bool spawncoridor = true;

        RectTransform Entrance = Instantiate(Room);
        Entrance.transform.SetParent(Panel.transform, false);
        Entrance.offsetMin = new Vector2(0, 0);
        //DungeonRoom.offsetMin = new Vector2(DungeonRoom.offsetMin.y, 300);
        Entrance.sizeDelta = new Vector2(70, 70);
        float RoomPositionX = Entrance.offsetMin.x;
        float RoomPositionY = Entrance.offsetMin.y;



        for (int RoomIndex = 0; RoomIndex < AllRoom; RoomIndex++)
        {
            DungeonRoom[RoomIndex] = Instantiate(Room);
            DungeonRoom[RoomIndex].transform.SetParent(Panel.transform, false);

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
                RoomPositionX = DungeonRoom[RoomIndex].offsetMin.x;
                RoomPositionY = DungeonRoom[RoomIndex].offsetMin.y;
                IndexCoridor += 1;
                
            } while (!RoomPosition);
            
                DungeonRoom[RoomIndex].sizeDelta = new Vector2(70, 70);
                yield return new WaitForSeconds(.1f);

        }
        SizePanel();
        ScrollPanel.horizontalNormalizedPosition = 0.5f;
        ScrollPanel.verticalNormalizedPosition = 0.5f;
    }
    public void randposition(int i,float RoomPositionX,float RoomPositionY)
    {
        
        switch (randpos)
        {
            case 1:
                //North
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX, RoomPositionY + 150);
                break;
            case 2:
                //East
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX + 150, RoomPositionY);
                break;
            case 3:
                //South
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX, RoomPositionY - 150);
                break;
            case 4:
                //West
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX - 150, RoomPositionY);
                break;
        }
        
    }

    public bool CheckRoom(int RoomIndex, bool RoomPosition)
    {
        for (int PrevRoomIndex = 0; PrevRoomIndex < RoomIndex; PrevRoomIndex++) //Check Room
        {

            if (DungeonRoom[RoomIndex].offsetMin == DungeonRoom[PrevRoomIndex].offsetMin || DungeonRoom[RoomIndex].offsetMin == new Vector2(0,0))
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

        for (int j=0; j<AllRoom; j++)
        {
            if (DungeonRoom[j].offsetMin.y > PanelTop)//Top
            {
                PanelTop = DungeonRoom[j].offsetMin.y; 
            }

            if (DungeonRoom[j].offsetMin.x > PanelRight) //Right
            {
                PanelRight = DungeonRoom[j].offsetMin.x;
            }

            if(DungeonRoom[j].offsetMin.y < PanelBottom)//Bottom
            {
                PanelBottom = DungeonRoom[j].offsetMin.y;
            }

            if(DungeonRoom[j].offsetMin.x < PanelLeft)//Left
            {
                PanelLeft = DungeonRoom[j].offsetMin.x;
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

    
}

