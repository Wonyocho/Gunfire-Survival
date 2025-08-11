/// <summary>
/// 점수 정책: 어떤 적이 사망했을 때 몇 점을 부여할지 결정.
/// OCP를 위해 정책을 교체 가능하게 설계.
/// </summary>
public interface IScorePolicy
{
    float GetScoreForEnemy(EnemyHealth enemy);
}

/// <summary>
/// 기본 정책: 처치 시 적의 MaxHP 만큼 점수 부여.
/// </summary>
public class MaxHpScorePolicy : IScorePolicy
{
    public float GetScoreForEnemy(EnemyHealth enemy)
    {
        if (enemy == null) return 0f;
        return enemy.MaxHP;
    }
}
