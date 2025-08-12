using UnityEngine;

// 돌격소총 ACX-7 구현 (히트스캔)
public class ACX_7 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "ACX-7";
    public override float Damage => 70f;
    public override float FireRate => 10f;
    public override int MagazineSize => 25;
    public override float ReloadTime => 2f;
    public override int PenetrationCount => 2;
    public override float SpreadAngle => 0f;
}
