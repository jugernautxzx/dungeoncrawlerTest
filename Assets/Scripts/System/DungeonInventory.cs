using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DungeonInventory
{

    List<Loot> itemLoot = new List<Loot>();
    public Button[] consumableButton = new Button[10];
    public Text[] treasureItem = new Text[20];
    int countButtonConsumable = 0;
    int countTreasureItem = 0;

    public void getLoot(string itemId, int amount, ItemType type)
    {
        if (itemLoot.Exists(t => t.itemId == itemId))
        {
            itemLoot.Find(t => t.itemId.Equals(itemId)).amount += amount;

        }
        else
        {
            if (type == ItemType.Consumable)
            {
                itemLoot.Add(new Loot(itemId, amount, type, countButtonConsumable));
                consumableButton[countButtonConsumable] = Button.Instantiate(DungeonModel.consumableItem);
                consumableButton[countButtonConsumable].transform.SetParent(DungeonModel.consumableContent);
                consumableButton[countButtonConsumable].transform.localScale = new Vector2(1, 1);
                consumableButton[countButtonConsumable].name = itemId;
                //consumableButton[countButtonConsumable].onClick.AddListener(delegate { useItem(itemId); });
                countButtonConsumable += 1;
            }
            else
            {
                itemLoot.Add(new Loot(itemId, amount, type, countTreasureItem));
                treasureItem[countTreasureItem] = Text.Instantiate(DungeonModel.treasureItem);
                treasureItem[countTreasureItem].transform.SetParent(DungeonModel.treasureContent);
                treasureItem[countTreasureItem].transform.localScale = new Vector2(1, 1);
                treasureItem[countTreasureItem].name = itemId;
                countTreasureItem += 1;
            }
        }

        UpdateItemText();
    }

    public void DropAction(string itemId)
    {
        if (itemLoot.Exists(t => t.itemId == itemId))
        {
            itemLoot.Find(t => t.itemId.Equals(itemId)).amount -= 1;
            CheckItemQuantity(itemId);
        }
        UpdateItemText();
    }

    public void DropAllAction(string itemId)
    {
        if (itemLoot.Exists(t => t.itemId == itemId))
        {
            itemLoot.Find(t => t.itemId.Equals(itemId)).amount = 0;
            CheckItemQuantity(itemId);
        }
    }

    public void UpdateItemText()
    {
        foreach (Loot loot in itemLoot)
        {
            if (loot.type == ItemType.Consumable)
            {
                consumableButton[loot.index].GetComponentInChildren<Text>().text = ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount;
                consumableButton[loot.index].GetComponentInChildren<Text>().fontSize = 25;
            }
            else
            {
                treasureItem[loot.index].text = ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount;
                treasureItem[loot.index].GetComponentInChildren<Text>().fontSize = 25;
            }

        }
    }

    public void CheckItemQuantity(string itemId)
    {
        if (itemLoot.Exists(t => t.itemId == itemId) && itemLoot.Find(t => t.itemId.Equals(itemId)).amount == 0)
        {
            if (itemLoot.Find(t => t.itemId.Equals(itemId)).type == ItemType.Consumable)
            {
                UnityEngine.Object.Destroy(consumableButton[itemLoot.Find(t => t.itemId.Equals(itemId)).index].gameObject);
            }
            else
            {
                UnityEngine.Object.Destroy(treasureItem[itemLoot.Find(t => t.itemId.Equals(itemId)).index].gameObject);
            }

            itemLoot.RemoveAt(itemLoot.FindIndex(t => t.itemId.Equals(itemId)));
        }
    }

    public void useItem(string id)
    {
        Debug.Log("use something");
    }

    public void WinDungeonLoot()
    {
        foreach (Loot loot in itemLoot)
        {
            PlayerSession.GetProfile().AddItem(loot.itemId, loot.amount);
        }
        PlayerSession.GetInstance().SaveSession();
    }

}
