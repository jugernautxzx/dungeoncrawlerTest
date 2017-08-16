using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IDungeonSelect
{
    void OnDungeonSelectClicked(int index);
}

public class DungeonSelectItemUI : MonoBehaviour, IPointerClickHandler {

    public Text dungeonName;
    public Text dungeonLevel;
    public IDungeonSelect iDunSelect;

    public void SetDungeonSelectListener(IDungeonSelect listener)
    {
        iDunSelect = listener;
    }

    public void SetDungeonInfo(UIDungeonInfo info)
    {
        dungeonName.text = info.stageName;
        dungeonLevel.text =  "Level " + info.stageLvMin + " ~ " + info.stageLvMax;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        iDunSelect.OnDungeonSelectClicked(transform.GetSiblingIndex());
    }
}
