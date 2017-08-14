using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnclickItem : MonoBehaviour, IPointerClickHandler
{
    DungeonControl dungeonControl;
    public RectTransform alertDropAll;

    RectTransform dropAllAlert = new RectTransform();

    OnclickItem()
    {
        dungeonControl = DungeonGenerator.dungeonControl;
    }

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
        else if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
    }

    public void DropItem()
    {
        dungeonControl.DropItemControl(gameObject.name);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void DropAllItem()
    {
        dungeonControl.DropAllItemControl(gameObject.name);
    }

    public void PopAlertDropAll()
    {
        dropAllAlert = Instantiate(alertDropAll);
        dropAllAlert.transform.SetParent(DungeonModel.inventoryContent.transform, false);
    }

    public void YesDropAll()
    {
        //Destroy(dropAllAlert.gameObject);
        Debug.Log(gameObject.name);
        DropAllItem();
    }

    public void NoDropAll()
    {
        //Destroy(dropAllAlert.gameObject);
    }
}
