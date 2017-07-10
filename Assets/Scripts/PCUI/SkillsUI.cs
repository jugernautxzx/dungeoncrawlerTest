using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SetSkillInterface
{
    void EquipSelectedSkill(int index);
}

public class SkillsUI : MonoBehaviour {

    public GameObject prefab;
    public Transform container;

    List<string> names;
    List<string> desc;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadAllAvailableActives()
    {

    }

    void LoadAllAvailablePassives()
    {

    }

    void ClearList()
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
    }
}
