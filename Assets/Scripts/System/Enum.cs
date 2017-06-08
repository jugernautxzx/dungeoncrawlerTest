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
    pdamage, mdamage, heal, buff, debuff
}

public enum SpecialEffect : int
{
    none, lifeleech
}

public enum Element : int
{
    none, physical, fire, water, wind, earth
}