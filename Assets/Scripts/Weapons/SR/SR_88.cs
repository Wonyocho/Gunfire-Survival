using UnityEngine;

// 스나이퍼 라이플 SR-88 구현 (히트스캔)
public class SR_88 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "SR-88";
    public override float Damage => 80f;
    public override float FireRate => 1.5f;
    public override int MagazineSize => 10;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 2;
    public override float SpreadAngle => 0f;
}
