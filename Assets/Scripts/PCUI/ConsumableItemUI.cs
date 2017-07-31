using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ConsumableInterface
{
}

public class ConsumableItemUI : MonoBehaviour, ConsumableItemInterface
{
    public const int FILTER_ALL = 0;
    public const int FILTER_CONSUMABLE = 1;
    public const int FILTER_SKILL = 2;
    public const int FILTER_TREASURE = 3;
    public const int FILTER_KEY = 4;

    public Dropdown filter;
    public GameObject viewPort;
    public GameObject prefab;

    TooltipInterface tooltip;
    List<int> sorting = new List<int>();

    // Use this for initialization
    void Start()
    {
        filter.onValueChanged.AddListener(ChangeFilter);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetListener(MainMenuUI ui)
    {
        tooltip = ui;
    }

    public void ChangeFilter(int code)
    {
        switch (code)
        {
            case FILTER_ALL:
                break;
            case FILTER_CONSUMABLE:
                break;
            case FILTER_KEY:
                break;
            case FILTER_SKILL:
                break;
            case FILTER_TREASURE:
                break;
        }
        UpdateSorting();
        UpdateList();
    }

    void UpdateSorting()
    {
        sorting.Clear();
        for(int i=0; i<PlayerSession.GetProfile().itemsId.Count; i++)
        {
            sorting.Add(i);
        }
    }

    void UpdateList()
    {
        StartCoroutine(LoadItemUI());
    }

    IEnumerator LoadItemUI()
    {
        yield return null;
        foreach(Transform child in viewPort.transform)
        {
            child.gameObject.SetActive(false);
        }
        for(int i=0; i<sorting.Count; i++)
        {
            if (i < viewPort.transform.childCount)
                UpdateItem(sorting[i], viewPort.transform.GetChild(i).GetComponent<ConsumableListItemUI>());
            else
                CreateNewItem(sorting[i]);
        }
    }

    void UpdateItem(int index, ConsumableListItemUI ui)
    {
        ui.gameObject.SetActive(true);
        ui.SetModel(ItemManager.GetInstance().GetItem(PlayerSession.GetProfile().itemsId[index]), PlayerSession.GetProfile().itemsOwned[index]);
    }

    void CreateNewItem(int index)
    {
        GameObject item = Instantiate(prefab, viewPort.transform, false);
        item.GetComponent<ConsumableListItemUI>().SetInterface(this, tooltip);
        UpdateItem(index, item.GetComponent<ConsumableListItemUI>());
    }

    public void OnItemClicked(int index)
    {

    }

}
