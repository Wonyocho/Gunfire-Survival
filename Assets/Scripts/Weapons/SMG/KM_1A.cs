using UnityEngine;

// 기관단총 KM-1A 구현 (히트스캔)
public class KM_1A : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "KM-1A";
    public override float Damage => 30f;
    public override float FireRate => 25f;
    public override int MagazineSize => 30;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 5f;
}
