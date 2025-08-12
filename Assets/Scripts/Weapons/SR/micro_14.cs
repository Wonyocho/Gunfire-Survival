using UnityEngine;

// 스나이퍼 라이플 micro-14 구현 (히트스캔)
public class micro_14 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "micro-14";
    public override float Damage => 70f;
    public override float FireRate => 1.5f;
    public override int MagazineSize => 30;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 2;
    public override float SpreadAngle => 0f;
}
