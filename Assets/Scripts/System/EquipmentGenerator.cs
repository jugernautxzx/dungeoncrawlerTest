using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGenerator
{

    public Equipment GenerateWeapon(int tier)
    {
        Equipment generated = new Equipment();
        generated.attribute = new Attribute();
        generated.bonus = new BonusAttribute();
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
            default:
                eq.name = eq.weapon.ToString();
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
        eq.bonus.attack = Random.Range(2 + tier, 5 + tier);
        eq.bonus.defense = Random.Range(0, tier);
    }

    void DaggerAttribute(Equipment eq, int tier)
    {
        eq.attribute.agi = Random.Range(tier, tier + 2);
        eq.attribute.speed = 2;
    }

    void SwordBonus(Equipment eq)//TODO Incomplete
    {
        eq.bonus.attack = Random.Range(4, 8);
    }
}
