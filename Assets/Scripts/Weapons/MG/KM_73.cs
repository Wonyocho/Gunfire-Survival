using UnityEngine;

// 기관총 KM-73 구현 (히트스캔)
public class KM_73 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "KM-73";
    public override float Damage => 50f;
    public override float FireRate => 13f; // 고정
    public override int MagazineSize => 100;
    public override float ReloadTime => 10f; // 고정
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 10f;
}
