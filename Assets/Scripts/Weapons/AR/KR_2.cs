using UnityEngine;

// 돌격소총 KR-2 구현 (히트스캔)
public class KR_2 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.AR;
    public override WeaponRank Rank => WeaponRank.One;
    public override string WeaponName => "KR-2";
    public override float Damage => 35f;
    public override float FireRate => 10f; // 고정
    public override int MagazineSize => 30;
    public override float ReloadTime => 2f; // 고정
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 0f;
}
