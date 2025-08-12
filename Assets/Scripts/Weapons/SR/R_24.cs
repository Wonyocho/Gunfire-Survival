using UnityEngine;

// 스나이퍼 라이플 R-24 구현 (히트스캔)
public class R_24 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "R-24";
    public override float Damage => 150f;
    public override float FireRate => 1.5f;
    public override int MagazineSize => 7;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 5;
    public override float SpreadAngle => 0f;
}
