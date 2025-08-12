using UnityEngine;

// 기관단총 Mzi-Pro 구현 (히트스캔)
public class Mzi_Pro : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "Mzi-Pro";
    public override float Damage => 15f;
    public override float FireRate => 25f;
    public override int MagazineSize => 35;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 20f;
}
