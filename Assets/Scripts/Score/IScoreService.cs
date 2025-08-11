/// <summary>
/// 점수 및 킬 카운트를 관리하는 서비스 계약.
/// SRP: 점수 로직만 담당. OCP: 정책 교체 가능. DIP: 인터페이스 의존.
/// </summary>
public interface IScoreService
{
    int TotalKills { get; }
    float TotalScore { get; }

    // UI 등 외부 소비자를 위한 변경 이벤트
    event System.Action<float> ScoreChanged;
    event System.Action<int> KillChanged;

    void AddScore(float amount);
    void AddKill(int count = 1);

    void ResetAll();
}

/// <summary>
/// 간단한 메모리 내 구현. UI가 구독할 이벤트를 노출.
/// </summary>
public class ScoreService : IScoreService
{
    public int TotalKills { get; private set; }
    public float TotalScore { get; private set; }

    public event System.Action<float> ScoreChanged; // 현재 총점
    public event System.Action<int> KillChanged;    // 현재 총 킬 수

    public void AddScore(float amount)
    {
        if (amount <= 0f) return;
        TotalScore += amount;
        ScoreChanged?.Invoke(TotalScore);
    }

    public void AddKill(int count = 1)
    {
        if (count <= 0) return;
        TotalKills += count;
        KillChanged?.Invoke(TotalKills);
    }

    public void ResetAll()
    {
        TotalKills = 0;
        TotalScore = 0f;
        ScoreChanged?.Invoke(TotalScore);
        KillChanged?.Invoke(TotalKills);
    }
}
