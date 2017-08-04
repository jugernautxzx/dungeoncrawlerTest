using UnityEngine;

public class EquipmentGenerator
{
    public readonly string[] TIER_NAMES = {"Wooden", "Iron", "Steel"};

    EqSlot genSlot = EqSlot.MainHand;
    Weapon genWeapon = Weapon.Dagger;
    int genTier = 1;

    //=================================================================================== GENERATOR SETTING ===============================================================
    public void Randomize()
    {
        genSlot = (EqSlot)Random.Range(0, 5);
        if (genSlot == EqSlot.MainHand)
            genWeapon = (Weapon)Random.Range(2, 11);
    }

    public EquipmentGenerator SetEquipmentSlot(EqSlot slot)
    {
        genSlot = slot;
        return this;
    }

    public EquipmentGenerator SetWeapon(Weapon w)
    {
        genWeapon = w;
        return this;
    }

    public EquipmentGenerator SetTier(int tier)
    {
        genTier = tier;
        return this;
    }

    public Equipment Generate()
    {
        Equipment generated = new Equipment();
        generated.slot = genSlot;
        if (genSlot == EqSlot.MainHand)
            generated.weapon = genWeapon;
        generated.id = GenerateId();
        return generated;
    }

    string GenerateId()
    {
        System.DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        double unix = (System.DateTime.Now - epoch).TotalMilliseconds;
        return unix.ToString();
    }
    //=============================================================================END OF GENERATOR SETTING ===============================================================

    //========================================================================================== Weapon generator ==========================================================

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
                throw new System.Exception("Unknown equipment type");
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
                break;
            case Weapon.Sword2H:
                break;
            case Weapon.Tome:
                break;
            case Weapon.Wand:
                break;
            default:
                throw new System.Exception("Unknown weapon type");
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
    //===================================================================== END OF WEAPON GENERATOR =============================================================================================


}
