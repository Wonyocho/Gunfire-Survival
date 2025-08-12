using UnityEngine;

// 스나이퍼 라이플 M82A1 구현 (히트스캔, 고관통)
public class M82A1 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override string WeaponName => "M82A1";
    public override float Damage => 300f;
    public override float FireRate => 1.5f; // 약 0.67초/발
    public override int MagazineSize => 10;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 20;
    public override float SpreadAngle => 0f;
}
