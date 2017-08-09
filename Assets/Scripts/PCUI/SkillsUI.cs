using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SetSkillInterface
{
    void EquipSelectedSkill(string id);
}

public class SkillsUI : MonoBehaviour, SkillItemInterface
{

    public GameObject prefab;
    public Transform container;

    List<string> ids = new List<string>();
    List<string> desc = new List<string>();

    SetSkillInterface setInterface;
    TooltipInterface tooltip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInterface(SetSkillInterface impl, TooltipInterface tooltip)
    {
        setInterface = impl;
        this.tooltip = tooltip;
    }

    public void LoadAllAvailableActives(List<string> learned, List<string> equiped)
    {
        ClearList();
        ids.Add("None");
        desc.Add("Remove active skill");

        if (learned != null)
            foreach (string id in learned)
            {
                if (!equiped.Contains(id))
                {
                    ids.Add(id);
                    desc.Add(ActiveSkillManager.GetInstance().GetActive(id).info);
                }
            }

        for (int i = 0; i < ids.Count; i++)
        {
            if (i < container.transform.childCount)
                UpdateItem(ids[i], desc[i], i);
            else
                PopulateList(ids[i], desc[i]);
        }
    }

    public void LoadAllAvailablePassives()
    {
        ClearList();
    }

    public void ClearList()
    {
        ids.Clear();
        desc.Clear();
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    void UpdateItem(string name, string desc, int index)
    {
        container.transform.GetChild(index).GetComponent<SkillsItemUI>().SetSkill(name, desc);
    }

    void PopulateList(string name, string desc)
    {
        GameObject instan = Instantiate(prefab, container, false);
        instan.GetComponent<SkillsItemUI>().SetSkill(name, desc);
        instan.GetComponent<SkillsItemUI>().SetInterface(this);
        instan.GetComponent<SkillsItemUI>().SetTooltip(tooltip);
    }

    public void OnItemClicked(string id)
    {
        setInterface.EquipSelectedSkill(id);
    }
}
