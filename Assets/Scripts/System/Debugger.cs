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
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.passives = new List<string>();
        return model;
    }

    public static void GenerateMainHand()
    {
        int tier = Random.Range(1, 10);
        EquipmentGenerator gen = new EquipmentGenerator();
        Equipment generated = gen.GenerateWeapon(tier);
        PlayerSession.GetInventory().list.Add(generated);
    }

    public static void GenerateOffHand()
    {
        Equipment generated = new Equipment();
        generated.attribute = new Attribute();
        generated.battle = new BattleAttribute();
        generated.id = Time.timeSinceLevelLoad.ToString() + Time.deltaTime.ToString();
        generated.name = "Parrying dagger";
        generated.slot = EqSlot.OffHand;
        generated.attribute.speed = 2;
        PlayerSession.GetInventory().list.Add(generated);
    }

}
