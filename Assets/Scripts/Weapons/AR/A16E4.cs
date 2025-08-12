using UnityEngine;

// 돌격소총 A16E4 구현 (히트스캔)
public class A16E4 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "A16E4";
    public override float Damage => 50f;
    public override float FireRate => 10f;
    public override int MagazineSize => 30;
    public override float ReloadTime => 2f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 0f;
}
