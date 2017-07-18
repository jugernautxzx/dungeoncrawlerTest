using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGenerator
{

    public Equipment GenerateWeapon(int tier)
    {
        Equipment generated = new Equipment();
        generated.attribute = new Attribute();
        generated.battle = new BattleAttribute();
        generated.id = Time.timeSinceLevelLoad.ToString() + Time.deltaTime.ToString();
        generated.slot = EqSlot.MainHand;
        RandomizeWeaponType(generated);
        GenerateName(generated, tier);
        RandomizedAttribute(generated, tier);
        RandomizedBonus(generated, tier);
        return generated;
    }

    void RandomizeWeaponType(Equipment equip)
    {
        //equip.weapon = (Weapon)Random.Range(2, 11);
        equip.weapon = Weapon.Dagger;
    }

    void RandomizedBonus(Equipment eq, int tier)
    {
        switch (eq.weapon)
        {
            case Weapon.Bow:
                break;
            case Weapon.Crossbow:
                break;
            case Weapon.Dagger:
                DaggerBonus(eq, tier);
                break;
            case Weapon.Shield:
                break;
            case Weapon.Spear:
                break;
            case Weapon.Staff:
                break;
            case Weapon.Sword:
                break;
            case Weapon.Sword2H:
                break;
            case Weapon.Tome:
                break;
            case Weapon.Wand:
                break;
            default:
                Debug.Log("Unidentified weapon type");
                break;
        }
    }

    void RandomizedAttribute(Equipment eq, int tier)
    {
        switch (eq.weapon)
        {
            case Weapon.Bow:
                break;
            case Weapon.Crossbow:
                break;
            case Weapon.Dagger:
                DaggerAttribute(eq, tier);
                break;
            case Weapon.Shield:
                break;
            case Weapon.Spear:
                break;
            case Weapon.Staff:
                break;
            case Weapon.Sword:
                SwordBonus(eq);
                break;
            case Weapon.Sword2H:
                break;
            case Weapon.Tome:
                break;
            case Weapon.Wand:
                break;
            default:
                Debug.Log("Unidentified weapon type");
                break;
        }
    }

    void GenerateName(Equipment eq, int tier)
    {
        //TODO to be determined
        switch (tier)
        {
            case 1:
                eq.name = "Iron " + eq.weapon.ToString();
                break;
            case 2:
                eq.name = "Steel " + eq.weapon.ToString();
                break;
            default:
                eq.name = tier.ToString() + " tiered " + eq.weapon.ToString();
                break;
        }
        int seed = Random.Range(0, 5);
        switch (seed)
        {
            case 0:
                eq.name += " of swiftness";
                break;
            case 1:
                eq.name = "Sharp " + eq.name;
                break;
            case 2:
                eq.name = "Fine " + eq.name;
                break;
            case 3:
                eq.name = "Heavy " + eq.name;
                break;
            case 4:
                eq.name = "Dull " + eq.name;
                break;
            case 5:
                eq.name = "Jagged " + eq.name;
                break;
        }
    }

    void DaggerBonus(Equipment eq, int tier)
    {
        eq.battle.basePAtk = Random.Range(((tier - 1) * 2) + 1, 3 + (tier * 2));
        eq.battle.basePDef= Random.Range(tier - 1, tier);
        eq.battle.baseMDef = Random.Range(0, 1);
        eq.battle.baseMatk= Random.Range(tier - 1, tier * 2);
    }

    void DaggerAttribute(Equipment eq, int tier)
    {
        eq.attribute.agi = Random.Range(tier, tier + 2);
        eq.attribute.speed = 2;
    }

    void SwordBonus(Equipment eq)//TODO Incomplete
    {
    }
}
