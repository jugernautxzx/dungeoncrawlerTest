using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem{

    public const string CONSUMABLE_HP = "HP";

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

    public static void UseConsumableInDungeon(ItemModel item, CharacterModel target)
    {
        string[] split = item.value.Split('|');
        switch (split[0])
        {
            case CONSUMABLE_HP:
                HealCharacter(split[1], target);
                break;
        }
    }

    static void HealCharacter(string value, CharacterModel target)
    {
        target.battleAttribute.ModifyHp(int.Parse(value));
    }
}
