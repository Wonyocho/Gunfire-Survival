using UnityEngine;

// 스나이퍼 라이플 R700 구현 (히트스캔, 다중 관통)
public class R700 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override string WeaponName => "R700";
    public override float Damage => 100f;
    public override float FireRate => 1f; // 1초/발
    public override int MagazineSize => 7;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 10;
    public override float SpreadAngle => 0f;
}

