using UnityEngine;

public class EquipmentGenerator
{
    public readonly string[] TIER_NAMES = { "Old","Wooden", "Iron", "Steel"};
    public readonly string[] DAGGER = {"Knife", "Dagger", "Combat knife", "Baselard"};

    EqSlot genSlot = EqSlot.MainHand;
    Weapon genWeapon = Weapon.Dagger;
    int genTier = 0;
    string presetName;
    Rarity setRarity;

    //=================================================================================== GENERATOR SETTING ===============================================================
    public void Randomize()
    {
        genSlot = (EqSlot)Random.Range(0, 5);
        if (genSlot == EqSlot.MainHand)
            genWeapon = (Weapon)Random.Range(2, 11);
    }

    public EquipmentGenerator SetName(string name)
    {
        presetName = name;
        return this;
    }

    public EquipmentGenerator SetRarity(Rarity rare)
    {
        setRarity = rare;
        return this;
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
        generated.name = GenerateName();
        if (genSlot == EqSlot.MainHand)
            generated.weapon = genWeapon;
        generated.id = GenerateId();
        generated.rarity = (int)setRarity;
        generated.battle = new BattleAttribute();
        generated.attribute = new Attribute();
        return generated;
    }

    string GenerateId()
    {
        System.DateTime epoch = new System.DateTime(2000, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        double unix = (System.DateTime.Now - epoch).TotalMilliseconds;
        return unix.ToString();
    }

    string GenerateName()
    {
        if (presetName == null)
            return genWeapon.ToString();
        else
            return presetName;
    }

    //=============================================================================END OF GENERATOR SETTING ===============================================================
    //===============================================================================START GENERATING======================================================================

    //========================================================================END OF START GENERATING======================================================================
    //========================================================================================== Weapon generator ==========================================================

    void RandomizedBonus(Equipment eq)
    {
        switch (eq.weapon)
        {
            case Weapon.Bow:
                break;
            case Weapon.Crossbow:
                break;
            case Weapon.Dagger:
                DaggerBonus(eq);
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

    void RandomizedAttribute(Equipment eq)
    {
        switch (eq.weapon)
        {
            case Weapon.Bow:
                break;
            case Weapon.Crossbow:
                break;
            case Weapon.Dagger:
                DaggerAttribute(eq);
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

    void DaggerBonus(Equipment eq)
    {
        eq.battle.basePAtk = Random.Range(((genTier - 1) * 2) + 1, 3 + (genTier * 2));
        eq.battle.basePDef= Random.Range(genTier - 1, genTier);
        eq.battle.baseMDef = Random.Range(0, 1);
        eq.battle.baseMatk= Random.Range(genTier - 1, genTier * 2);
    }

    void DaggerAttribute(Equipment eq)
    {
        eq.attribute.agi = Random.Range(genTier, genTier + 2);
        eq.attribute.speed = 2;
    }
    //===================================================================== END OF WEAPON GENERATOR =============================================================================================


}
