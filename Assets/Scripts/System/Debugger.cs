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
        model.passives = new List<string>();
        return model;
    }

    public static void GenerateMainHand()
    {
        Equipment generated = new Equipment();
        generated.attribute = new Attribute();
        generated.bonus = new BonusAttribute();
        generated.id = Time.timeSinceLevelLoad.ToString() + Time.deltaTime.ToString();
        generated.name = "Iron dagger";
        generated.slot = EqSlot.MainHand;
        generated.weapon = Weapon.Dagger;
        generated.bonus.attack = 6;
        generated.attribute.speed = 2;
        PlayerSession.GetInventory().list.Add(generated);
    }

    public static void GenerateOffHand()
    {
        Equipment generated = new Equipment();
        generated.attribute = new Attribute();
        generated.bonus = new BonusAttribute();
        generated.id = Time.timeSinceLevelLoad.ToString() + Time.deltaTime.ToString();
        generated.name = "Parrying dagger";
        generated.slot = EqSlot.OffHand;
        generated.bonus.defense = 2;
        generated.attribute.speed = 2;
        PlayerSession.GetInventory().list.Add(generated);
    }

}
