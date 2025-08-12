using UnityEngine;

// 스나이퍼 라이플 MR-25 구현 (히트스캔)
public class MR_25 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "MR-25";
    public override float Damage => 90f;
    public override float FireRate => 1.5f;
    public override int MagazineSize => 20;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 3;
    public override float SpreadAngle => 0f;
}
