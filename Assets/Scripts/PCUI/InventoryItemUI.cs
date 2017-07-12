using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface InventoryItemInterface
{
    void OnItemClicked(int index);
}

public interface InventoryItemShowInterface
{
    void OnShowEquipmentStatus(Equipment model, Vector2 pos);
    void OnCloseEquipmentStatus();
}

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public Text eqName;
    public Text eqType;
    public Text eqAttack;

    Equipment model;

    InventoryItemInterface selectedImpl;
    InventoryItemShowInterface showImpl;

	// Use this for initialization
	void Start () {
		
	}

    public void SetInterface(InventoryItemInterface ii)
    {
        selectedImpl = ii;
    }

    public void SetShowInterface(InventoryItemShowInterface iis)
    {
        showImpl = iis;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetModel(Equipment model)
    {
        this.model = model;
        eqName.text = model.name;
        eqAttack.text = "Attack " + model.bonus.attack;
        eqType.text = model.weapon.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedImpl != null)
            selectedImpl.OnItemClicked(transform.GetSiblingIndex());
        if (showImpl != null)
            showImpl.OnCloseEquipmentStatus();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (showImpl != null)
            showImpl.OnShowEquipmentStatus(model, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showImpl != null)
            showImpl.OnCloseEquipmentStatus();
    }


}
