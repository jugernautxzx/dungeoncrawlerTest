using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DungeonInventory {

    string itemText;
    List<Loot> itemLoot = new List<Loot>();
    public Button[] consumableButton = new Button[10];
    int countButtonConsumable = 0;

    public void getLoot(string itemId, int amount, ItemType type)
    {
        if (itemLoot.Exists(t=>t.itemId==itemId))
        {
            itemLoot.Find(t=>t.itemId.Contains(itemId)).amount+=amount;
            
        }
        else
        {
            itemLoot.Add(new Loot(itemId, amount, type, countButtonConsumable));
            if(type==ItemType.Consumable)
            {
                consumableButton[countButtonConsumable] = Button.Instantiate(DungeonModel.consumableItem);
                consumableButton[countButtonConsumable].transform.SetParent(DungeonModel.consumableContent);
                consumableButton[countButtonConsumable].transform.localScale = new Vector2(1,1);
                consumableButton[countButtonConsumable].name = itemId;
                countButtonConsumable += 1;
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
        DungeonModel.inventoryTreasureText.text = "";
        itemText = "";
        foreach (Loot loot in itemLoot)
        {
            if (loot.type == ItemType.Consumable)
            {
                consumableButton[loot.index].GetComponentInChildren<Text>().text = ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount;
                consumableButton[loot.index].GetComponentInChildren<Text>().fontSize = 25;
            }
            else
            {
                itemText += ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount + "\n";
            }

        }

        DungeonModel.inventoryTreasureText.text = itemText;
    }

    public void CheckItemQuantity(string itemId)
    {
        if (itemLoot.Exists(t => t.itemId == itemId) && itemLoot.Find(t => t.itemId.Equals(itemId)).amount == 0)
        {
            UnityEngine.Object.Destroy(consumableButton[itemLoot.Find(t => t.itemId.Equals(itemId)).index].gameObject);
            itemLoot.RemoveAt(itemLoot.FindIndex(t => t.itemId.Equals(itemId)));
        }
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
