using UnityEngine;

// 스나이퍼 라이플 R700 구현 (히트스캔, 다중 관통)
public class R700 : IWeapon
{
    // ---- 설정(스탯) ----
    public WeaponCategory Category => WeaponCategory.SR;
    public string WeaponName => "R700";
    public float Damage => 100f;
    public float FireRate => 1f;          // 1초/발
    public int MagazineSize => 7;
    public float ReloadTime => 3f;        // 초
    public int PenetrationCount => 10;    // 경로상 최대 10명 적중
    public float SpreadAngle => 0f;       // 탄퍼짐 없음

    // 내부 상수
    const float MaxRange = 40f;           // 히트스캔 사거리(프로젝트 기준)
    const float MinDirMag = 0.0001f;

    // ---- 상태 ----
    int currentAmmo;
    bool isReloading;
    float reloadTimer;
    float fireCooldown; // 쿨다운 남은 시간(초)

    // ---- 이벤트 ----
    public System.Action<IWeapon> OnAmmoChanged { get; set; }
    public System.Action<IWeapon> OnReloadStarted { get; set; }
    public System.Action<IWeapon> OnReloadCompleted { get; set; }

    // ---- 상태 프로퍼티 ----
    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;
    public bool CanFire => !isReloading && currentAmmo > 0 && fireCooldown <= 0f;

    // ---- 생성 ----
    public R700()
    {
        currentAmmo = MagazineSize;
        isReloading = false;
        reloadTimer = 0f;
        fireCooldown = 0f;
    }

    // ---- 메인 로직 ----
    public bool TryFire(Vector2 origin, Vector2 direction, LayerMask targetMask)
    {
        if (!CanFire) {
            if (!isReloading && currentAmmo <= 0) StartReload();
            return false;
        }

        if (direction.sqrMagnitude < MinDirMag * MinDirMag)
            return false;
        Vector2 dir = direction.normalized;

        // 스프레드(정확도 0이므로 기본적으로 없음)
        float spread = SpreadAngle;
        if (spread > 0f)
        {
            float half = spread * 0.5f;
            float addDeg = Random.Range(-half, half);
            float rad = addDeg * Mathf.Deg2Rad;
            float cs = Mathf.Cos(rad);
            float sn = Mathf.Sin(rad);
            dir = new Vector2(dir.x * cs - dir.y * sn, dir.x * sn + dir.y * cs);
        }

        // 발사 처리(히트스캔, 다중 관통)
        int hitsApplied = 0;
        var hits = Physics2D.RaycastAll(origin, dir, MaxRange, targetMask);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var h in hits)
        {
            var eh = h.collider ? (h.collider.GetComponent<EnemyHealth>() ?? h.collider.GetComponentInParent<EnemyHealth>()) : null;
            if (eh)
            {
                eh.TakeDamage(Damage);
                hitsApplied++;
                if (hitsApplied >= PenetrationCount) break;
            }
        }

        // 탄약/쿨다운 갱신
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        fireCooldown = 1f / Mathf.Max(0.0001f, FireRate);
        OnAmmoChanged?.Invoke(this);

        if (currentAmmo == 0 && !isReloading)
            StartReload();

        return hitsApplied > 0;
    }

    public void StartReload()
    {
        if (isReloading) return;
        if (currentAmmo >= MagazineSize) return;
        isReloading = true;
        reloadTimer = ReloadTime;
        OnReloadStarted?.Invoke(this);
    }

    public void Update(float deltaTime)
    {
        if (fireCooldown > 0f)
        {
            fireCooldown -= deltaTime;
            if (fireCooldown < 0f) fireCooldown = 0f;
        }

        if (isReloading)
        {
            reloadTimer -= deltaTime;
            if (reloadTimer <= 0f)
            {
                isReloading = false;
                reloadTimer = 0f;
                currentAmmo = MagazineSize;
                OnReloadCompleted?.Invoke(this);
                OnAmmoChanged?.Invoke(this);
            }
        }
    }
}

