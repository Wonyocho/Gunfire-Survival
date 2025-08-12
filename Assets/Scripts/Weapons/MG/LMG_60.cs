using UnityEngine;

// 기관총 LMG-60 구현 (히트스캔)
public class LMG_60 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "LMG-60";
    public override float Damage => 65f;
    public override float FireRate => 13f;
    public override int MagazineSize => 100;
    public override float ReloadTime => 10f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 5f;
}
