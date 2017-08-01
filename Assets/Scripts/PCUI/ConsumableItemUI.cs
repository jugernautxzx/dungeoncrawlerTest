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
    ConfirmationDialogInterface dialog;
    List<int> filteredList = new List<int>();
    int selectedItemIndex;

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
        dialog = ui;
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
        UpdateFilteredList();
        UpdateList();
    }

    void UpdateFilteredList()
    {
        filteredList.Clear();
        for (int i = 0; i < PlayerSession.GetProfile().itemsId.Count; i++)
        {
            filteredList.Insert(0, i);
        }
    }

    void UpdateList()
    {
        StartCoroutine(LoadItemUI());
    }

    IEnumerator LoadItemUI()
    {
        yield return null;
        foreach (Transform child in viewPort.transform)
        {
            child.gameObject.SetActive(false);
        }
        for (int i = 0; i < filteredList.Count; i++)
        {
            if (i < viewPort.transform.childCount)
                UpdateItem(filteredList[i], viewPort.transform.GetChild(i).GetComponent<ConsumableListItemUI>());
            else
                CreateNewItem(filteredList[i]);
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
        selectedItemIndex = filteredList[index];//TODO Fix dialog
        switch (ItemManager.GetInstance().GetItem(PlayerSession.GetProfile().itemsId[selectedItemIndex]).item)
        {
            case ItemType.Consumable:
                DialogUseConsumable();
                break;
            case ItemType.SkillBook:
                break;
            case ItemType.Treasure:
                DialogSellTreasure();
                break;
        }
    }

    void DialogUseConsumable()
    {
        dialog.RequestConfirmationDialog("Request to use item?", OnItemUseYes, null, null);
    }

    void DialogSellTreasure()
    {
        dialog.RequestConfirmationDialog("Sell all " + ItemManager.GetInstance().GetItem(PlayerSession.GetProfile().itemsId[selectedItemIndex]).name + "?", OnTreasureSellYes, null, null);
    }

    void OnItemUseYes()
    {

        PlayerSession.GetProfile().RemoveItem(selectedItemIndex, 1);
        UpdateFilteredList();
        UpdateList();
    }

    void OnTreasureSellYes()
    {
        PlayerSession.GetProfile().RemoveItem(selectedItemIndex, PlayerSession.GetProfile().itemsOwned[selectedItemIndex]);
        UpdateFilteredList();
        UpdateList();
    }

    void UseConsumableItem()
    {

    }
}
