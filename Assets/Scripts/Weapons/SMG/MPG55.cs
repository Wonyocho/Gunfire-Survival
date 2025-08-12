using UnityEngine;

// 기관단총 MPG55 구현 (히트스캔)
public class MPG55 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SMG;
    public override WeaponRank Rank => WeaponRank.Two;
    public override string WeaponName => "MPG55";
    public override float Damage => 20f;
    public override float FireRate => 25f;
    public override int MagazineSize => 30;
    public override float ReloadTime => 1.5f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 10f;
}
