using UnityEngine;

// 스나이퍼 라이플 MR-70 구현 (히트스캔)
public class MR_70 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "MR-70";
    public override float Damage => 100f;
    public override float FireRate => 1.5f; // 고정
    public override int MagazineSize => 7;
    public override float ReloadTime => 3f; // 고정
    public override int PenetrationCount => 3;
    public override float SpreadAngle => 0f;
}
