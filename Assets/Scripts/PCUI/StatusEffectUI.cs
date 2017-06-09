using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusEffectUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    string infoId="InfoPoison";

    public void SetInfoId(string id)
    {
        infoId = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("BattleUI").GetComponent<BattleUI>().UpdateInfoId(infoId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //GameObject.Find("BattleUI").GetComponent<BattleUI>().UpdateInfoId(infoId);
    }
}
