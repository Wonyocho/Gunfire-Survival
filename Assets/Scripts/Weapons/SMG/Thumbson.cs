using UnityEngine;

// 기관단총 Thumbson 구현 (히트스캔)
public class Thumbson : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "Thumbson";
    public override float Damage => 20f;
    public override float FireRate => 25f;
    public override int MagazineSize => 45;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 15f;
}
