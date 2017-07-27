using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipedSkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string description;
    TooltipInterface tooltip;

    public void SetDescription(string desc)
    {
        description = desc;
    }

    public void SetInterface(TooltipInterface tip)
    {
        tooltip = tip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
