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

}
