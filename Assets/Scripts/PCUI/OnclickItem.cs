using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnclickItem : MonoBehaviour, IPointerClickHandler
{
    DungeonControl dungeonControl;
    public RectTransform alertDropAll;
    public Button dropAllYes;
    public Button dropAllNo;

    RectTransform alertDropAllObj;
    Button dropAllYesObj;
    Button dropAllNoObj;


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
        alertDropAllObj = Instantiate(alertDropAll);
        alertDropAllObj.transform.SetParent(DungeonModel.inventoryContent.transform,false);
        dropAllYesObj = Instantiate(dropAllYes);
        dropAllYesObj.transform.SetParent(alertDropAllObj.transform.GetChild(0),false);
        dropAllYesObj.onClick.AddListener(YesDropAll);
        dropAllNoObj = Instantiate(dropAllNo);
        dropAllNoObj.transform.SetParent(alertDropAllObj.transform.GetChild(0), false);
        dropAllNoObj.onClick.AddListener(NoDropAll);
    }

    public void YesDropAll()
    {
        DropAllItem();
        Destroy(alertDropAllObj.gameObject);
    }

    public void NoDropAll()
    {
        Destroy(alertDropAllObj.gameObject);
    }
}
