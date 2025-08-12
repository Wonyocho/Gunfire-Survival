using UnityEngine;

// 돌격소총 AKX-47 구현 (히트스캔)
public class AKX_47 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "AKX-47";
    public override float Damage => 40f;
    public override float FireRate => 10f;
    public override int MagazineSize => 30;
    public override float ReloadTime => 2f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 0f;
}
