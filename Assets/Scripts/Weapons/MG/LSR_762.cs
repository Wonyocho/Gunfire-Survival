using UnityEngine;

// 기관총 LSR-762 구현 (히트스캔)
public class LSR_762 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.MG;
    public override WeaponRank Rank => WeaponRank.Three;
    public override string WeaponName => "LSR-762";
    public override float Damage => 60f;
    public override float FireRate => 13f;
    public override int MagazineSize => 150;
    public override float ReloadTime => 10f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 20f;
}
