using UnityEngine;

// 돌격소총 STAR 구현 (히트스캔)
public class STAR : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "STAR";
    public override float Damage => 45f;
    public override float FireRate => 10f;
    public override int MagazineSize => 30;
    public override float ReloadTime => 2f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 0f;
}
