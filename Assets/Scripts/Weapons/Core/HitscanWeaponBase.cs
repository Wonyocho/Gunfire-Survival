using UnityEngine;

/// <summary>
/// 히트스캔 무기의 공통 로직을 담은 추상 기본 클래스.
/// Template Method 패턴으로 발사/리로드/쿨다운 처리 표준화.
/// </summary>
public abstract class HitscanWeaponBase : IWeapon
{
    // ---- 무기 스탯(서브클래스에서 지정) ----
    public abstract WeaponCategory Category { get; }
    public virtual WeaponRank Rank => WeaponRank.One; // 기본 등급(필요 시 서브클래스에서 재정의)
    public abstract string WeaponName { get; }
    public abstract float Damage { get; }
    public abstract float FireRate { get; }
    public abstract int MagazineSize { get; }
    public abstract float ReloadTime { get; }
    public abstract int PenetrationCount { get; }
    public abstract float SpreadAngle { get; }

    // 추가 옵션
    protected virtual float MaxRange => 40f; // 기본 사거리
    protected virtual int Pellets => 1;      // 샷건 등 다중 펠릿용
    protected virtual bool ReturnTrueWhenFiredRegardlessOfHit => true; // 히트 여부와 무관하게 발사 여부 반환

    // ---- 상태 ----
    protected int currentAmmo;
    protected bool isReloading;
    protected float reloadTimer;
    protected float fireCooldown;

    // ---- 이벤트 ----
    public System.Action<IWeapon> OnAmmoChanged { get; set; }
    public System.Action<IWeapon> OnReloadStarted { get; set; }
    public System.Action<IWeapon> OnReloadCompleted { get; set; }
    
    // VFX용 실제 발사 방향 이벤트 (origin, actualDirection, weapon)
    public System.Action<Vector2, Vector2, IWeapon> OnActualShot { get; set; }

    // ---- 상태 프로퍼티 ----
    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;
    public bool CanFire => !isReloading && currentAmmo > 0 && fireCooldown <= 0f;

    protected HitscanWeaponBase()
    {
        currentAmmo = MagazineSize;
        isReloading = false;
        reloadTimer = 0f;
        fireCooldown = 0f;
    }

    public virtual bool TryFire(Vector2 origin, Vector2 direction, LayerMask targetMask)
    {
        if (!CanFire)
        {
            if (!isReloading && currentAmmo <= 0) StartReload();
            return false;
        }

        if (direction.sqrMagnitude < 1e-8f) return false;
        var dir = ApplySpread(direction.normalized);

        int totalHits = 0;
        for (int i = 0; i < Mathf.Max(1, Pellets); i++)
        {
            Vector2 shotDir = (i == 0) ? dir : ApplySpread(direction.normalized);
            totalHits += FireSingleRay(origin, shotDir, targetMask);
            
            // 실제 발사 방향으로 VFX 이벤트 발송
            OnActualShot?.Invoke(origin, shotDir, this);
        }

        // 탄약/쿨다운 갱신
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        fireCooldown = 1f / Mathf.Max(0.0001f, FireRate);
        OnAmmoChanged?.Invoke(this);

        if (currentAmmo == 0 && !isReloading)
            StartReload();

        if (ReturnTrueWhenFiredRegardlessOfHit) return true;
        return totalHits > 0;
    }

    protected virtual int FireSingleRay(Vector2 origin, Vector2 dir, LayerMask mask)
    {
        int hitsApplied = 0;
        if (PenetrationCount <= 1)
        {
            var hit = Physics2D.Raycast(origin, dir, MaxRange, mask);
            if (hit.collider)
            {
                var dmg = hit.collider.GetComponent<IDamageable>()
                          ?? hit.collider.GetComponentInParent<IDamageable>();
                if (dmg != null)
                {
                    dmg.TakeDamage(Damage);
                    hitsApplied = 1;
                }
            }
        }
        else
        {
            var hits = Physics2D.RaycastAll(origin, dir, MaxRange, mask);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            foreach (var h in hits)
            {
                var dmg = h.collider ? (h.collider.GetComponent<IDamageable>() ?? h.collider.GetComponentInParent<IDamageable>()) : null;
                if (dmg != null)
                {
                    dmg.TakeDamage(Damage);
                    hitsApplied++;
                    if (hitsApplied >= PenetrationCount) break;
                }
            }
        }
        return hitsApplied;
    }

    protected virtual Vector2 ApplySpread(Vector2 dir)
    {
        float spread = SpreadAngle;
        if (spread <= 0f) return dir;
        float half = spread * 0.5f;
        float addDeg = Random.Range(-half, half);
        float rad = addDeg * Mathf.Deg2Rad;
        float cs = Mathf.Cos(rad);
        float sn = Mathf.Sin(rad);
        return new Vector2(dir.x * cs - dir.y * sn, dir.x * sn + dir.y * cs);
    }

    public virtual void StartReload()
    {
        if (isReloading) return;
        if (currentAmmo >= MagazineSize) return;
        isReloading = true;
        reloadTimer = ReloadTime;
        OnReloadStarted?.Invoke(this);
    }

    public virtual void Update(float deltaTime)
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
