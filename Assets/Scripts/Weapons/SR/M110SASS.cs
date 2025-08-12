using UnityEngine;

// 스나이퍼 라이플 M110 SASS 구현 (히트스캔)
public class M110SASS : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override string WeaponName => "M110SASS";
    public override float Damage => 75f;
    public override float FireRate => 2f; // 0.5초/발
    public override int MagazineSize => 20;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 3;
    public override float SpreadAngle => 0f;
}
