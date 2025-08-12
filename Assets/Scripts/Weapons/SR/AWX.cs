using UnityEngine;

// 스나이퍼 라이플 AWX 구현 (히트스캔)
public class AWX : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SR;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "AWX";
    public override float Damage => 500f;
    public override float FireRate => 1.5f;
    public override int MagazineSize => 5;
    public override float ReloadTime => 3f;
    public override int PenetrationCount => 20;
    public override float SpreadAngle => 0f;
}
