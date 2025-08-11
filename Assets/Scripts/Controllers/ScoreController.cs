using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// EnemyHealth의 사망 이벤트를 구독하여 점수/킬을 갱신.
/// SRP: 점수 집계만 담당. DIP: IScoreService, IScorePolicy에 의존.
/// </summary>
public class ScoreController : MonoBehaviour
{
    // 단일 인스턴스 보장: 필드 초기화로 생성 시점 확정
    private IScoreService _service = new ScoreService();
    private IScorePolicy _policy = new MaxHpScorePolicy();

    // 신규 스폰되는 적 구독 관리용(중복 구독 방지)
    private readonly HashSet<EnemyHealth> _subscribed = new HashSet<EnemyHealth>();

    // Awake에서 추가 로직이 필요 없다면 생략 가능
    void Awake()
    {
    }

    void OnEnable()
    {
        // 현재 씬에 존재하는 모든 EnemyHealth를 구독
        ResubscribeToEnemies();
        // 이후 생성되는 적은 EnemyRegister가 All에 추가되므로 주기적으로 동기화
        InvokeRepeating(nameof(ResubscribeToEnemies), 0.25f, 0.25f);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(ResubscribeToEnemies));
        // 안전하게 모두 언구독
        foreach (var eh in _subscribed)
        {
            if (eh != null) eh.Died -= OnEnemyDied;
        }
        _subscribed.Clear();
    }

    void ResubscribeToEnemies()
    {
        // 신규 등록된 적에 대해서만 이벤트 연결
        foreach (var eh in EnemyRegistry.All)
        {
            if (eh == null) continue;
            if (_subscribed.Add(eh))
            {
                eh.Died += OnEnemyDied;
            }
        }
        // 파괴된 참조 정리(가비지 엔트리 제거)
        _subscribed.RemoveWhere(e => e == null);
    }

    void OnEnemyDied(EnemyHealth enemy)
    {
        float score = _policy.GetScoreForEnemy(enemy);
        _service.AddScore(score);
        _service.AddKill(1);
        // 구독 집합에서 제거(재사용 방지)
        _subscribed.Remove(enemy);
        // Debug.Log($"Score: {_service.TotalScore}, Kills: {_service.TotalKills}");
    }

    // 외부(UI)에서 접근이 필요할 수 있어 프로퍼티로 노출
    public IScoreService Service => _service;
}

// SubscribedFlag MonoBehaviour 제거: AddComponent로 동적 추가가 불가(파일명/접근 제한)하고,
// HashSet으로 중복 구독을 안전하게 방지합니다.
