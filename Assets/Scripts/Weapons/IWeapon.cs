using UnityEngine;

// 무기 카테고리 열거형
public enum WeaponCategory
{
    HG,  // Handgun
    SMG, // Submachine Gun
    AR,  // Assault Rifle
    SR,  // Sniper Rifle
    MG   // Machine Gun
}

/// <summary>
/// 모든 무기가 구현해야 하는 기본 인터페이스
/// </summary>
public interface IWeapon
{
    // 메타 정보
    WeaponCategory Category { get; } // 무기 카테고리

	// 기본 스탯
		string WeaponName { get; } // 무기 이름
    float Damage { get; } // 무기의 데미지
    float FireRate { get; } // 무기의 발사 속도
    int MagazineSize { get; } // 탄창 크기
    float ReloadTime { get; } // 재장전 시간
    int PenetrationCount { get; } // 관통 가능 횟수
    float SpreadAngle { get; } // 탄퍼짐 각도 (도 단위)

    // 현재 상태
    int CurrentAmmo { get; } // 현재 남은 탄약
    bool IsReloading { get; } // 재장전 중 여부
    bool CanFire { get; } // 발사 가능 여부

    // 기본 액션
    bool TryFire(Vector2 origin, Vector2 direction, LayerMask targetMask); // 발사 시도
    void StartReload(); // 재장전 시작
    void Update(float deltaTime); // 업데이트

    // 이벤트
    System.Action<IWeapon> OnAmmoChanged { get; set; }
    System.Action<IWeapon> OnReloadStarted { get; set; }
    System.Action<IWeapon> OnReloadCompleted { get; set; }
}
