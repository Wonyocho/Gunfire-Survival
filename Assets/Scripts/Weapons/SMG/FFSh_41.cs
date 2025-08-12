using UnityEngine;

// 기관단총 FFSh-41 구현 (히트스캔)
public class FFSh_41 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "FFSh-41";
    public override float Damage => 15f;
    public override float FireRate => 25f;
    public override int MagazineSize => 50;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 15f;
}
