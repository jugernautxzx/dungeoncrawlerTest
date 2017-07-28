using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsumableListItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Text itemName;
    public Text itemTotal;
    public Text itemType;

    string desc;
    TooltipInterface tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
