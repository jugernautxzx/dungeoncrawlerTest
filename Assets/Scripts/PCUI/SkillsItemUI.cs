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

public class SkillsItemUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    string id;
    string desc;
    SkillItemInterface impl;
    TooltipInterface tooltip;

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
