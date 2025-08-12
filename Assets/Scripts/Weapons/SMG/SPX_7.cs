using UnityEngine;

// 기관단총 SPX-7 구현 (히트스캔)
public class SPX_7 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "SPX-7";
    public override float Damage => 25f;
    public override float FireRate => 25f;
    public override int MagazineSize => 40;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 0f;
}
