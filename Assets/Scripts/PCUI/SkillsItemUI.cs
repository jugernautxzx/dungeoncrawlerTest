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

public class SkillsItemUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    string id;
    string desc;
    SkillItemInterface impl;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
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

    public void OnPointerEnter(PointerEventData eventData)
    {//TODO add floating description here
        Debug.Log("Pointer entering: " + desc);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!id.Equals("None"))
            impl.OnItemClicked(id);
        else
            impl.OnItemClicked("");
    }
}
