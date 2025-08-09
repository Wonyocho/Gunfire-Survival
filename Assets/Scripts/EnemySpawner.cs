using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;            // Player Transform
    [SerializeField] private GameObject enemyPrefab;      // 반드시 Project의 Prefab 참조
    [SerializeField] private BoxCollider2D worldBounds;   // 큰 맵 경계(선택)

    [Header("Timing")]
    [SerializeField] private float startDelay = 1.0f;     // 시작 후 첫 스폰 지연
    [SerializeField] private float spawnInterval = 0.5f;  // 스폰 간격

    [Header("Limits")]
    [SerializeField] private int maxEnemies = 500;

    [Header("Spawn Distance (Annulus)")]
    [Tooltip("플레이어로부터 이 거리보다 가까우면 스폰하지 않음")]
    [SerializeField] private float minSpawnDistance = 10f;
    [Tooltip("플레이어로부터 이 거리보다 멀면 스폰하지 않음")]
    [SerializeField] private float maxSpawnDistance = 30f;

    private float nextTime;
    private int alive;

    void OnValidate()
    {
        if (maxSpawnDistance < minSpawnDistance + 0.1f)
            maxSpawnDistance = minSpawnDistance + 0.1f;
        if (minSpawnDistance < 0f) minSpawnDistance = 0f;
    }

    void Start()
    {
        nextTime = Time.time + startDelay;
    }

    void Update()
    {
        if (!player || !enemyPrefab) return;
        if (alive >= maxEnemies) return;

        if (Time.time >= nextTime)
        {
            Vector2 spawnPos;
            if (TryGetSpawnPosition(out spawnPos))
            {
                var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                alive++;
                enemy.AddComponent<OnDestroyNotify>().Init(() => alive--);
            }
            // 다음 스폰 예약
            nextTime = Time.time + spawnInterval;
        }
    }

    /// <summary>
    /// 플레이어 기준 [min, max] 반경의 도넛(annulus) 안에서,
    /// worldBounds 안쪽에 들어오는 위치를 고른다.
    /// </summary>
    bool TryGetSpawnPosition(out Vector2 pos)
    {
        pos = Vector2.zero;
        if (!player) return false;

        // 1) 우선 도넛 영역에서 N회 시도해보고, 그 점이 월드 경계 안이면 사용
        const int ATTEMPTS = 24;
        for (int i = 0; i < ATTEMPTS; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized; // 랜덤 방향
            float dist = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector2 cand = (Vector2)player.position + dir * dist;

            if (IsInsideWorld(cand))
            {
                pos = cand;
                return true;
            }
        }

        // 2) 실패 시: 가장 마지막 방향 기준으로 경계 클램프 + 최소거리 보정
        //    (도넛 밖/경계 밖 문제를 약식으로 해결)
        {
            Vector2 dir = (Random.insideUnitCircle.normalized);
            if (dir == Vector2.zero) dir = Vector2.right;

            Vector2 cand = (Vector2)player.position + dir * maxSpawnDistance;

            // 경계 클램프
            cand = ClampToWorld(cand);

            // 플레이어와 너무 가까우면 최소 거리만큼 밀어냄
            Vector2 fromPlayer = cand - (Vector2)player.position;
            float d = fromPlayer.magnitude;
            if (d < Mathf.Max(0.001f, minSpawnDistance))
            {
                fromPlayer = fromPlayer.normalized * minSpawnDistance;
                cand = (Vector2)player.position + fromPlayer;
                cand = ClampToWorld(cand);
            }

            // 최종 검증: 여전히 너무 멀지/가깝지 않은지
            float finalDist = Vector2.Distance(cand, player.position);
            if (finalDist >= minSpawnDistance && finalDist <= maxSpawnDistance && IsInsideWorld(cand))
            {
                pos = cand;
                return true;
            }
        }

        // 그래도 실패면 스폰 포기(다음 틱에 다시 시도)
        return false;
    }

    bool IsInsideWorld(Vector2 p)
    {
        if (!worldBounds) return true; // 경계 미지정이면 항상 true
        var b = worldBounds.bounds;
        return (p.x >= b.min.x && p.x <= b.max.x && p.y >= b.min.y && p.y <= b.max.y);
    }

    Vector2 ClampToWorld(Vector2 p)
    {
        if (!worldBounds) return p;
        var b = worldBounds.bounds;
        p.x = Mathf.Clamp(p.x, b.min.x + 0.5f, b.max.x - 0.5f);
        p.y = Mathf.Clamp(p.y, b.min.y + 0.5f, b.max.y - 0.5f);
        return p;
    }
}

/// <summary>
/// 프리팹이 파괴될 때 현재 스포너 alive 카운트를 줄이기 위한 유틸
/// </summary>
public class OnDestroyNotify : MonoBehaviour
{
    private System.Action onDestroyed;
    public void Init(System.Action callback) => onDestroyed = callback;
    void OnDestroy() => onDestroyed?.Invoke();
}
