using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnclickItem : MonoBehaviour, IPointerClickHandler
{
    DungeonControl dungeonControl;
    public RectTransform alertDropAll;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (gameObject.transform.GetChild(1).gameObject.activeInHierarchy)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
       
    }

    public void DropItem()
    {
        dungeonControl = DungeonGenerator.dungeonControl;
        dungeonControl.DropItemControl(gameObject.name);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void DropAllItem()
    {
        dungeonControl = DungeonGenerator.dungeonControl;
        dungeonControl.DropAllItemControl(gameObject.name);
    }

    public void PopAlertDropAll()
    {
        alertDropAll.gameObject.SetActive(true);
    }

    public void NoDropAll()
    {
        alertDropAll.gameObject.SetActive(false);
    }
}
