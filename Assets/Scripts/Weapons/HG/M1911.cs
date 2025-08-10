using UnityEngine;

// 단발 권총 M1911 구현 (히트스캔)
public class M1911 : IWeapon
{
    // ---- 설정(스탯) ----
    public WeaponCategory Category => WeaponCategory.HG;
    public string WeaponName => "M1911";
    public float Damage => 10f;
    public float FireRate => 2f;          // 0.5초/발
    public int MagazineSize => 7;
    public float ReloadTime => 2f;      // 초
    public int PenetrationCount => 1;     // 1명만 적중
    public float SpreadAngle => 1f;       // 도(deg)

    // 내부 상수
    const float MaxRange = 40f;           // 히트스캔 사거리
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
    public M1911()
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
            // 총알이 0이면 자동 리로드 시도(선택)
            if (!isReloading && currentAmmo <= 0) StartReload();
            return false;
        }

        // 방향 보정 및 탄퍼짐 적용
        if (direction.sqrMagnitude < MinDirMag * MinDirMag)
            return false;
        Vector2 dir = direction.normalized;
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

        // 발사 처리(히트스캔)
        int hitsApplied = 0;
        if (PenetrationCount <= 1)
        {
            var hit = Physics2D.Raycast(origin, dir, MaxRange, targetMask);
            if (hit.collider)
            {
                var eh = hit.collider.GetComponent<EnemyHealth>() ?? hit.collider.GetComponentInParent<EnemyHealth>();
                if (eh)
                {
                    eh.TakeDamage(Damage);
                    hitsApplied = 1;
                }
            }
        }
        else
        {
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
        }

        // 탄약/쿨다운 갱신
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        fireCooldown = 1f / Mathf.Max(0.0001f, FireRate);
        OnAmmoChanged?.Invoke(this);

        // 사격 후 탄약이 0이면 자동 리로드 시작(선택)
        if (currentAmmo == 0 && !isReloading)
            StartReload();

        return hitsApplied > 0; // 실제 명중 여부 반환(시각효과는 호출 측에서 처리 가능)
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
