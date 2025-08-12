using UnityEngine;

// 기관총 MGX-5 구현 (히트스캔)
public class MGX_5 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "MGX-5";
    public override float Damage => 55f;
    public override float FireRate => 13f;
    public override int MagazineSize => 200;
    public override float ReloadTime => 10f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 10f;
}
