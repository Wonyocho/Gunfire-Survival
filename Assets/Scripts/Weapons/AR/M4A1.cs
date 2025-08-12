using UnityEngine;

// 돌격소총 M4A1 구현 (히트스캔)
public class M4A1 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override string WeaponName => "M4A1";
    public override float Damage => 25f;
    public override float FireRate => 10f; // 0.1초/발
    public override int MagazineSize => 30;
    public override float ReloadTime => 2f; // 
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 1f;
}
