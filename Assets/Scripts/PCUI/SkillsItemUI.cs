using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface SkillItemInterface
{
    void OnItemClicked(string id);
}

public interface SkillItemTooltipInterface
{
    void ShowText(string desc);
    void HideText();
}

public class SkillsItemUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    string id;
    string desc;
    SkillItemInterface impl;
    TooltipInterface tooltip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInterface(SkillItemInterface skillInterface)
    {
        this.impl = skillInterface;
    }

    public void SetSkill(string id, string desc)
    {
        this.id = id;
        this.desc = desc;
        GetComponent<Text>().text = ActiveSkillManager.GetInstance().GetName(id);
    }

    public void SetTooltip(TooltipInterface tool)
    {
        tooltip = tool;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(desc);
    }

public void OnPointerClick(PointerEventData eventData)
{
    if (!id.Equals("None"))
        impl.OnItemClicked(id);
    else
        impl.OnItemClicked("");
        tooltip.HideTooltip();
}

public void OnPointerExit(PointerEventData eventData)
{
    tooltip.HideTooltip();
}
}
