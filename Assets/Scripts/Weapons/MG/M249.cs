using UnityEngine;

// 기관총 M249 구현 (히트스캔)
public class M249 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override string WeaponName => "M249";
    public override float Damage => 25f;
    public override float FireRate => 13f; // 0.076초/발
    public override int MagazineSize => 100;
    public override float ReloadTime => 10f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 1f;
}
