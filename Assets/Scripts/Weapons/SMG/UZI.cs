using UnityEngine;

// 기관단총 UZI 구현 (히트스캔)
public class UZI : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override string WeaponName => "UZI";
    public override float Damage => 15f;
    public override float FireRate => 100f; // 0.01초/발
    public override int MagazineSize => 40;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 1f;
}
