using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger
{

    public static void GenerateMainHand()
    {
        EquipmentGenerator gen = new EquipmentGenerator();
        PlayerSession.GetInventory().list.Add(gen.Generate());
    }

    public static void GenerateOffHand()
    {//TODO Deprecated
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
