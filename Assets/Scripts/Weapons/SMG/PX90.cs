using UnityEngine;

// 기관단총 PX90 구현 (히트스캔)
public class PX90 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "PX90";
    public override float Damage => 30f;
    public override float FireRate => 25f;
    public override int MagazineSize => 50;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 5f;
}
