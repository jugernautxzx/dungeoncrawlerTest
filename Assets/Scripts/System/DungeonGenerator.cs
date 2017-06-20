using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungeonGenerator : MonoBehaviour {

    public RectTransform Room;
    public RectTransform Panel;
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
                SizePanel(RoomIndex);
                Panel.offsetMin = new Vector2(-1000, -1000);//Left Bottom
                Panel.offsetMax = new Vector2(2000, 2000); //Right Top
                yield return new WaitForSeconds(.1f);

        }
    }
    public void randposition(int i,float RoomPositionX,float RoomPositionY)
    {
        
        switch (randpos)
        {
            case 1:
                //North
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX, RoomPositionY + 200);
                break;
            case 2:
                //East
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX + 200, RoomPositionY);
                break;
            case 3:
                //South
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX, RoomPositionY - 200);
                break;
            case 4:
                //West
                DungeonRoom[i].offsetMin = new Vector2(RoomPositionX - 200, RoomPositionY);
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
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY + 70);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;

            case 2:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorX);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX + 70, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY + 45);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;
            case 3:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorY);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX + 25, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY - 130);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;

            case 4:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorX);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].offsetMin = new Vector2(RoomPositionX - 130, 0);
                DungeonCoridor[IndexCoridor].offsetMax = new Vector2(0, RoomPositionY + 45);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;
        }
    }

    public void SizePanel(int RoomIndex)
    {
        float PanelTop = 0;
        float panelLeft = 0;
        float panelRight = 0;
        float panelBottom = 0;

        for (int j=0; j<=RoomIndex; j++)
        {
            if (DungeonRoom[j].offsetMin.y > DungeonRoom[RoomIndex].offsetMin.y)
            {
                PanelTop = DungeonRoom[j].offsetMin.y;
                Debug.Log(PanelTop);
            }

        }
        Debug.Log("hasil akhir: "+PanelTop);
    }
}

