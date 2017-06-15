using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungeonGenerator : MonoBehaviour {

    public RectTransform Room;
    public RectTransform Panel;
    public RectTransform CoridorX;
    public RectTransform CoridorY;
    int randpos;
    int IndexCoridor=0;
    float PrevRoomPositionX;
    float PrevRoomPositionY;

    RectTransform[] DungeonRoom = new RectTransform[100];
    RectTransform[] DungeonCoridor = new RectTransform[100];

    void Start () {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        bool position = true;
        bool spawncoridor = true;

        RectTransform  Entrance = Instantiate(Room);
        Entrance.transform.SetParent(Panel.transform, false);
        Entrance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        //DungeonRoom.offsetMin = new Vector2(DungeonRoom.offsetMin.y, 300);
        Entrance.sizeDelta = new Vector2(70,70);
        float RoomPositionX = Entrance.offsetMin.x;
        float RoomPositionY = Entrance.offsetMin.y;

        

        for (int i=0; i<20; i++ )
        {
            DungeonRoom[i] = Instantiate(Room);
            DungeonRoom[i].transform.SetParent(Panel.transform, false);
           
            do
            {
                randposition(i, RoomPositionX, RoomPositionY);
                
                for (int j=0; j<i; j++)
                {
                    if(DungeonRoom[i].GetComponent<RectTransform>().offsetMin == DungeonRoom[j].GetComponent<RectTransform>().offsetMin || DungeonRoom[i].GetComponent<RectTransform>().offsetMin == new Vector2(0,0))
                    {
                        position = false;
                        break;
                    }
                    else
                    {
                        position = true;
                    }

                }

                SpawnCoridor(IndexCoridor, RoomPositionX, RoomPositionY);

                for (int K=0;K<IndexCoridor;K++)
                {
                    
                    if (DungeonCoridor[K].GetComponent<RectTransform>().offsetMin==DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMin)
                    {
                        Destroy(DungeonCoridor[IndexCoridor].gameObject);
                        IndexCoridor -= 1;
                    }
                    
                }

                IndexCoridor += 1;
                   

            } while (position == false);
            DungeonRoom[i].sizeDelta = new Vector2(70, 70);
            RoomPositionX = DungeonRoom[i].offsetMin.x;
            RoomPositionY = DungeonRoom[i].offsetMin.y;


        }


    }
    public void randposition(int i,float RoomPositionX,float RoomPositionY)
    {
        PrevRoomPositionX = RoomPositionX;
        PrevRoomPositionY = RoomPositionY;
        randpos = Random.Range(1, 5);
        switch (randpos)
        {
            case 1:
                //North
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX, RoomPositionY + 200);
                break;
            case 2:
                //East
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX + 200, RoomPositionY);

                break;
            case 3:
                //South
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX, RoomPositionY - 200);
                break;
            case 4:
                //West
                DungeonRoom[i].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX - 200, RoomPositionY);
                break;
        }
        
    }
    public void SpawnCoridor(int IndexCoridor, float RoomPositionX, float RoomPositionY)
    {
        switch (randpos)
        {
            case 1:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorY);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX + 25, 0);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMax = new Vector2(0, RoomPositionY + 70);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;

            case 2:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorX);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX + 70, 0);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMax = new Vector2(0, RoomPositionY + 45);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;
            case 3:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorY);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX + 25, 0);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMax = new Vector2(0, RoomPositionY - 130);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;

            case 4:
                DungeonCoridor[IndexCoridor] = Instantiate(CoridorX);
                DungeonCoridor[IndexCoridor].transform.SetParent(Panel.transform, false);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMin = new Vector2(RoomPositionX - 130, 0);
                DungeonCoridor[IndexCoridor].GetComponent<RectTransform>().offsetMax = new Vector2(0, RoomPositionY + 45);
                DungeonCoridor[IndexCoridor].sizeDelta = new Vector2(260, 16);
                break;
        }
    }
}

