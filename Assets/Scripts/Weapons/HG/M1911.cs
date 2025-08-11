using UnityEngine;

// 단발 권총 M1911 구현 (히트스캔)
public class M1911 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.HG;
    public override string WeaponName => "M1911";
    public override float Damage => 10f;
    public override float FireRate => 2f;
    public override int MagazineSize => 7;
    public override float ReloadTime => 2f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 1f;
}
