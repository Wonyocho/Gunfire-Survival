/// <summary>
/// 읽기 전용 체력 정보를 제공하는 인터페이스.
/// SRP: 체력 상태 제공. ISP: 최소한의 계약. DIP: UI는 이 인터페이스에만 의존.
/// </summary>
public interface IHealthSource
{
    float CurrentHP { get; }
    float MaxHP { get; }

    // 체력 변화 시 통지 (current, max)
    event System.Action<float, float> HealthChanged;
}
