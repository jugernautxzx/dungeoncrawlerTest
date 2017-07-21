using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLoader
{

    public static CharacterModel LoadMonster(string id)
    {
        CharacterModel model = new CharacterModel();
        model.name = id;
        model.attribute = new Attribute();
        model.attribute.agi = Random.Range(1, 5);
        model.attribute.cons = Random.Range(1, 5);
        model.attribute.endurance = Random.Range(1, 5);
        model.attribute.intel = Random.Range(1, 5);
        model.attribute.wisdom = Random.Range(1, 5);
        model.attribute.str = Random.Range(1, 5);
        model.attribute.speed = Random.Range(1, 5);
        model.battleSetting = new BattleSetting();
        model.elemental = new ElementAttribute();
        model.actives = new List<string>();
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.passives = new List<string>();
        return model;
    }

    public static CharacterModel LoadMonsterData(string id, int level)
    {
        CharacterModel monster = XmlLoader.LoadFromXmlResource<CharacterModel>("Xml/Monster/" + id);
        monster.level = level;
        monster.name = "Lv. " + level + " " + monster.name;
        MonsterLevelUp(monster);
        return monster;
    }

    static void MonsterLevelUp(CharacterModel monster)
    {
        //monster.attribute.agi += Random.Range(0, MaxIncrease(monster.level, monster.attribute.agi));
        //monster.attribute.str += Random.Range(0, MaxIncrease(monster.level, monster.attribute.str));
        //monster.attribute.wisdom += Random.Range(0, MaxIncrease(monster.level, monster.attribute.wisdom));
        //monster.attribute.cons += Random.Range(0, MaxIncrease(monster.level, monster.attribute.cons));
        //monster.attribute.endurance += Random.Range(0, MaxIncrease(monster.level, monster.attribute.endurance));
        //monster.attribute.intel += Random.Range(0, MaxIncrease(monster.level, monster.attribute.intel));

        //monster.attribute.agi += MaxIncrease(monster.level, monster.attribute.agi);
        //monster.attribute.str += MaxIncrease(monster.level, monster.attribute.str);
        //monster.attribute.wisdom += MaxIncrease(monster.level, monster.attribute.wisdom);
        //monster.attribute.cons += MaxIncrease(monster.level, monster.attribute.cons);
        //monster.attribute.endurance += MaxIncrease(monster.level, monster.attribute.endurance);
        //monster.attribute.intel += MaxIncrease(monster.level, monster.attribute.intel);

        int points = (monster.level - 1) * 3;
        while (points > 0)
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    monster.attribute.agi++;
                    break;
                case 1:
                    monster.attribute.str++;
                    break;
                case 2:
                    monster.attribute.cons++;
                    break;
                case 3:
                    monster.attribute.endurance++;
                    break;
                case 4:
                    monster.attribute.wisdom++;
                    break;
                case 5:
                    monster.attribute.intel++;
                    break;
            }
            points--;
        }
    }

    static int MaxIncrease(int level, int val)
    {
        int max = 0;
        max = Mathf.RoundToInt(level * (val / 10f));
        return max;
    }

    public static void SaveMonster(CharacterModel model)
    {
        XmlSaver.SaveXmlToFile<CharacterModel>("/" + model.name + ".xml", model);
    }
}
