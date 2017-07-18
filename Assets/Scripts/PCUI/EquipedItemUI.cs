using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipedItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    InventoryItemShowInterface iisImpl;
    Equipment model;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInterface(InventoryItemShowInterface iis)
    {
        iisImpl = iis;
    }

    public void SetModel(Equipment model)
    {
        this.model = model;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (iisImpl != null)
            iisImpl.OnCloseEquipmentStatus();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (iisImpl != null && model != null)
            iisImpl.OnShowEquipmentStatus(model, eventData.position);
    }
}
