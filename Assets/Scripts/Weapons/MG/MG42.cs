using UnityEngine;

// 기관총 MG42 구현 (히트스캔)
public class MG42 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "MG42";
    public override float Damage => 60f;
    public override float FireRate => 13f;
    public override int MagazineSize => 120;
    public override float ReloadTime => 10f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 30f;
}
