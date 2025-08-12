using UnityEngine;

// 돌격소총 ACX 구현 (히트스캔)
public class ACX : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "ACX";
    public override float Damage => 55f;
    public override float FireRate => 10f;
    public override int MagazineSize => 35;
    public override float ReloadTime => 2f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 0f;
}
