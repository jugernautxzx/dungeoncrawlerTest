using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    string infoId="InfoPoison";
    string text;

    public void SetInfoId(string text, string id)
    {
        infoId = id;
        GetComponent<Text>().text = text;
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
