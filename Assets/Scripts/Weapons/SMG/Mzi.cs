using UnityEngine;

// 기관단총 Mzi 구현 (히트스캔)
public class Mzi : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "Mzi";
    public override float Damage => 15f;
    public override float FireRate => 25f; // 고정
    public override int MagazineSize => 25;
    public override float ReloadTime => 1.5f; // 고정
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 30f;
}
