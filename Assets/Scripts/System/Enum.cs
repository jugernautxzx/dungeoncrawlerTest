public enum Targetable
{
    self, singleparty, singleenemy, allparty, allenemy, everyone
}

public enum Target
{
    self, target, allparty, allenemy, everyone,randomparty, randomenemy, randomall
}

public enum Row
{
    all, front, back
}

public enum EffectType
{
    damage, heal, buff, debuff
}

public enum SpecialEffect : int
{
    none, lifeleech, poison
}

public enum Element : int
{
    none, physical, fire, water, wind, earth
}

public enum Weapon : int
{
    None, All, Dagger, Sword, Sword2H, Shield, Spear, Bow, Crossbow, Wand, Staff, Tome
}

public enum BuffTag : int
{
    Buff, Debuff, Poison, Regen, AttribUp, AttribDown
}

public enum EqSlot : int
{
    MainHand, OffHand, Head, Body, Accessory
}