using UnityEngine;

// 기관총 MG249 구현 (히트스캔)
public class MG249 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "MG249";
    public override float Damage => 50f;
    public override float FireRate => 13f;
    public override int MagazineSize => 150;
    public override float ReloadTime => 10f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 10f;
}
