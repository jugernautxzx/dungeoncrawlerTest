using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem{

    public static void LearnActiveSkills(CharacterModel chara, ItemModel skillBook)
    {
        if(skillBook.item == ItemType.SkillBook)
        {
            if (!chara.learnActive.Contains(skillBook.value))
                chara.learnActive.Add(skillBook.value);
        }
    }


    public static void SellItem(ItemModel treasure, int totalAmount)
    {
        int gold = treasure.gold * totalAmount;
        PlayerSession.GetProfile().Gold += gold;
    }
}
