using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLoader{

    public static CharacterModel LoadMonster(string name)
    {
        CharacterModel model = new CharacterModel();
        model.name = name;
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

    public static void SaveMonster(CharacterModel model)
    {
        XmlSaver.SaveXmlToFile<CharacterModel>("/" + model.name, model);
    }
}
