using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharListInterface {
    void ClickedItemIndex(int index);
}

public class CharList : MonoBehaviour, OnCharacterUIClicked {

    List<int> filteredList = new List<int>();

    public GameObject prefab;
    public UnityEngine.UI.Button disable;

    CharListInterface iCListInterface;

    void Start()
    {
        disable.onClick.AddListener(OnDisableButtonClick);
    }

    public void NoFilter()
    {
        filteredList.Clear();
        int i = 0;
        foreach(CharacterModel cModel in PlayerSession.GetProfile().characters)
        {
            filteredList.Add(i);
            i++;
        }
    }

    public void FilterActiveMembers()
    {
        filteredList.Clear();
        filteredList.Add(0);
        filteredList.Add(PlayerSession.GetProfile().party.member1);
        filteredList.Add(PlayerSession.GetProfile().party.member2);
        filteredList.Add(PlayerSession.GetProfile().party.member3);
    }

    void AddToFilteredList(int i)
    {
        if (i > 0)
            filteredList.Add(i);
    }

    public void FilterActiveLearnable(string activeId)
    {
        filteredList.Clear();
        int i = 0;
        foreach (CharacterModel cModel in PlayerSession.GetProfile().characters)
        {
            if(!cModel.learnActive.Contains(activeId))
                filteredList.Add(i);
            i++;
        }
    }

    void OnEnable()
    {
        disable.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        disable.gameObject.SetActive(false);
    }

    void OnDisableButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void SetListener(CharListInterface I)
    {
        iCListInterface = I;
    }

    public void StartUpdateAllMember()
    {
        StartCoroutine(UpdateAllMembers());
    }

    IEnumerator UpdateAllMembers()
    {
        yield return null;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        for(int i=0; i<filteredList.Count; i++)
        {
            if(i < transform.childCount)
            {
                UpdateItem(transform.GetChild(i).GetComponent<CharacterUI>(), filteredList[i]);
            }
            else
            {
                CreateNewItem(filteredList[i]);
            }
        }
    }

    void UpdateItem(CharacterUI cUI, int index)
    {
        cUI.gameObject.SetActive(true);
        cUI.LoadInformation(PlayerSession.GetProfile().characters[index]);
    }

    void CreateNewItem(int charIndex)
    {
        GameObject character = Instantiate(prefab, transform, false);
        character.GetComponent<CharacterUI>().LoadInformation(PlayerSession.GetProfile().characters[charIndex]);
        character.GetComponent<CharacterUI>().RemoveEquipmentButton();
        character.GetComponent<CharacterUI>().SetClickListener(this);
    }

    public void OnClicked(int index)
    {
        iCListInterface.ClickedItemIndex(filteredList[index]);
    }
}