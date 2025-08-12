using UnityEngine;

// 스나이퍼 라이플 R82A1 구현 (히트스캔)
public class R82A1 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "R82A1";
    public override float Damage => 250f;
    public override float FireRate => 1.5f;
    public override int MagazineSize => 10;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 10;
    public override float SpreadAngle => 0f;
}
