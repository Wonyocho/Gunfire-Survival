public static class WeaponFactory
{
    public static IWeapon Create(StartWeaponType type)
    {
        switch (type)
        {
            case StartWeaponType.M1911: return new M1911();
            case StartWeaponType.UZI:   return new UZI();
            case StartWeaponType.M4A1:  return new M4A1();
            case StartWeaponType.R700:  return new R700();
            case StartWeaponType.M249:  return new M249();
            case StartWeaponType.M1014: return new M1014();
            default: return new M1911();
        }
    }
}
