using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger
{

    public static CharacterModel GenerateCharacterModel(string name)
    {
        CharacterModel model = new CharacterModel();
        model.name = name;
        model.attribute = new Attribute();
        model.baseHP = 20;
        model.attribute.agi = 1;
        model.attribute.cons = 1;
        model.attribute.endurance = 1;
        model.attribute.intel = 1;
        model.attribute.wisdom = 1;
        model.attribute.str = 1;
        model.attribute.speed = 1;
        model.battleSetting = new BattleSetting();
        model.elemental = new ElementAttribute();
        model.actives = new List<string>();
        model.passives = new List<string>();
        return model;
    }

}
