using UnityEngine;

// 샷건 M1014 구현 (히트스캔, 다중 펠릿)
public class M1014 : HitscanWeaponBase
{
    public override WeaponCategory Category => WeaponCategory.SG;
    public override string WeaponName => "M1014";
    public override float Damage => 8f; // 펠릿 당 피해
    public override float FireRate => 1.2f;
    public override int MagazineSize => 7;
    public override float ReloadTime => 3.2f;
    public override int PenetrationCount => 1;
    public override float SpreadAngle => 6f;

    // 샷건 특화
    protected override int Pellets => 8; // 한 발에 8펠릿
    protected override bool ReturnTrueWhenFiredRegardlessOfHit => true;
}
