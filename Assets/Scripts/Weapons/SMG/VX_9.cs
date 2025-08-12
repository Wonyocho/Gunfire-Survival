using UnityEngine;

// 기관단총 VX-9 구현 (히트스캔)
public class VX_9 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "VX-9";
    public override float Damage => 25f;
    public override float FireRate => 25f;
    public override int MagazineSize => 35;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 5f;
}
