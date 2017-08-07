using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInventory {

    string itemText;
    List<Loot> itemLoot = new List<Loot>();
    public Button[] consumableButton = new Button[10];
    int countButtonConsumable = 0;

    public void getLoot(string itemId, int amount, ItemType type)
    {
        int countConsumableItem = 0;
        DungeonModel.inventoryTreasureText.text = "";
        itemText = "";

        if (itemLoot.Exists(t=>t.itemId==itemId))
        {
            itemLoot.Find(t=>t.itemId.Contains(itemId)).amount+=amount;
            
        }
        else
        {
            itemLoot.Add(new Loot(itemId, amount, type));
            if(type==ItemType.Consumable)
            {
                consumableButton[countButtonConsumable] = Button.Instantiate(DungeonModel.consumableItem);
                consumableButton[countButtonConsumable].transform.SetParent(DungeonModel.consumableContent);
                consumableButton[countButtonConsumable].transform.localScale = new Vector2(1,1);
                countButtonConsumable += 1;
            }
        }

        foreach (Loot loot in itemLoot)
        {
            if (loot.type==ItemType.Consumable)
            {
                consumableButton[countConsumableItem].GetComponentInChildren<Text>().text = ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount;
                consumableButton[countConsumableItem].GetComponentInChildren<Text>().fontSize = 25;
                countConsumableItem += 1;
            }
            else
            {
                itemText += ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount + "\n";
            }
            
        }

        DungeonModel.inventoryTreasureText.text = itemText;
        
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
