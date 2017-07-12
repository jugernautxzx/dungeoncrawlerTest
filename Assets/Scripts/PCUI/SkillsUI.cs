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

    List<string> names = new List<string>();
    List<string> desc = new List<string>();

    SetSkillInterface setInterface;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInterface(SetSkillInterface impl)
    {
        setInterface = impl;
    }

    public void LoadAllAvailableActives(List<string> learned, List<string> equiped)
    {
        ClearList();
        names.Add("None");
        desc.Add("Remove active skill");

        if (learned != null)
            foreach (string id in learned)
            {
                if (!equiped.Contains(id))
                {
                    names.Add(ActiveSkillManager.GetInstance().GetActive(id).name);
                    desc.Add(ActiveSkillManager.GetInstance().GetActive(id).info);
                }
            }

        for (int i = 0; i < names.Count; i++)
        {
            PopulateList(names[i], desc[i]);
        }
    }

    public void LoadAllAvailablePassives()
    {
        ClearList();
    }

    public void ClearList()
    {
        names.Clear();
        desc.Clear();
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    void PopulateList(string name, string desc)
    {
        GameObject instan = Instantiate(prefab, container, false);
        instan.GetComponent<SkillsItemUI>().SetSkill(name, desc);
        instan.GetComponent<SkillsItemUI>().SetInterface(this);
    }

    public void OnItemClicked(string id)
    {
        setInterface.EquipSelectedSkill(id);
    }
}
