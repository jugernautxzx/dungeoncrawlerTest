using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInventoryUI : MonoBehaviour
{
    class LootItem
    {
        public string id;
        public int qty;
        public int weight;
        public ItemType iType;
    }

    public DungeonCharPanel dungeonCharPanel;
    public ConfirmationDialog confirmationDialog;
    public GameObject consumablePrefab;
    public GameObject treasurePrefab;
    public GameObject consumableViewport;
    public GameObject treasureViewport;

    List<LootItem> lootItemList = new List<LootItem>();

    // Use this for initialization
    void Start()
    {
        lootItemList = new List<LootItem>();
    }

    public void AddItem(string id, int qty)
    {
        if (IsAlreadyHasItem(id))
        {
            lootItemList[IndexOf(id)].qty += qty;
        }
        else
        {
            LootItem loot = new LootItem();
            loot.id = id;
            loot.qty = qty;
            loot.iType = ItemManager.GetInstance().GetItem(id).item;
            loot.weight = ItemManager.GetInstance().GetItem(id).weight;
            lootItemList.Add(loot);
        }
    }

    public void RemoveItem(int index)
    {

        UpdateAllItemList();
    }

    public void RemoveAllItem()
    {
        UpdateAllItemList();
    }

    bool IsAlreadyHasItem(string id)
    {
        return lootItemList.Exists(item => item.id == id);
    }

    int IndexOf(string id)
    {
        return lootItemList.FindIndex(item => item.id == id);
    }

    void UpdateAllItemList()
    {
        HideAllItem();
        UpdateConsumableList();
        UpdateTreasureList();
    }

    void UpdateConsumableList()
    {
        int index = 0;
        foreach (LootItem item in lootItemList)
        {
            if (item.iType == ItemType.Consumable)
            {
                if (consumableViewport.transform.childCount < index)
                    CreateConsumableItem(item);
                else
                    UpdateListItem(consumableViewport, index, item);
                index++;
            }
        }
    }

    void HideAllItem()
    {
        foreach (Transform child in treasureViewport.transform)
            child.gameObject.SetActive(false);
        foreach (Transform child in consumableViewport.transform)
            child.gameObject.SetActive(false);
    }

    void CreateConsumableItem(LootItem loot)
    {
        GameObject cons = Instantiate(consumablePrefab, consumableViewport.transform, false);
        cons.GetComponentInChildren<Text>().text = ItemManager.GetInstance().GetItem(loot.id).name;
    }

    void UpdateListItem(GameObject list, int index, LootItem item)
    {
        list.transform.GetChild(index).gameObject.SetActive(true);
        list.transform.GetChild(index).GetComponent<Text>().text = ItemManager.GetInstance().GetItem(item.id).name;
    }

    void UpdateTreasureList()
    {
        int index = 0;
        foreach (LootItem item in lootItemList)
        {
            if (item.iType == ItemType.Treasure)
            {
                if (treasureViewport.transform.childCount < index)
                    CreateTreasureItem(item);
                else
                    UpdateListItem(treasureViewport, index, item);
                index++;
            }
        }
    }

    void CreateTreasureItem(LootItem item)
    {
        GameObject treasure = Instantiate(treasurePrefab, treasureViewport.transform, false);
        treasure.GetComponentInChildren<Text>().text = ItemManager.GetInstance().GetItem(item.id).name;
    }

    void CreateConfirmationDialog()
    {
    }
}
