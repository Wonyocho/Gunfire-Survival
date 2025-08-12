using UnityEngine;

// 기관단총 UZI 구현 (히트스캔)
public class UZI : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override string WeaponName => "UZI";
    public override float Damage => 15f;
    public override float FireRate => 25f; // 0.04초/발
    public override int MagazineSize => 40;
    public override float ReloadTime => 1.5f; // 1.5초 재장전
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 1f;
}
