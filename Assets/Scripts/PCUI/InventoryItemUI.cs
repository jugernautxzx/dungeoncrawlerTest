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

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public Text eqName;
    public Text eqType;
    public Text eqAttack;

    Equipment model;

    InventoryItemInterface impl;

	// Use this for initialization
	void Start () {
		
	}

    public void SetInterface(InventoryItemInterface ii)
    {
        impl = ii;
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
        if (impl != null)
            impl.OnItemClicked(transform.GetSiblingIndex());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
