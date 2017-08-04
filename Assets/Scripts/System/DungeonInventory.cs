using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInventory {

    string itemText;

    List<Loot> itemLoot = new List<Loot>();

    public void getLoot(string itemId, int amount, Text item)
    {
        item.text = "";
        itemText = "";
        if (itemLoot.Exists(t=>t.itemId==itemId))
        {
            itemLoot.Find(t=>t.itemId.Contains(itemId)).amount+=amount;
            
        }
        else
        {
            itemLoot.Add(new Loot(itemId, amount));
        }

        foreach (Loot loot in itemLoot)
        {
            itemText += ItemManager.GetInstance().GetItem(loot.itemId).name + " x " + loot.amount + "\n";
        }
        Debug.Log(itemLoot.Count);

        item.text = itemText;
        
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
