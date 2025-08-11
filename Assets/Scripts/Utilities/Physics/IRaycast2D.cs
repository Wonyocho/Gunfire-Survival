using UnityEngine;

/// <summary>
/// 2D 물리 레이캐스트 추상화. 테스트 대체/모킹 용이.
/// </summary>
public interface IRaycast2D
{
    RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask);
    RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask);
}

/// <summary>
/// Unity Physics2D를 사용하는 기본 구현.
/// </summary>
public class UnityRaycast2D : IRaycast2D
{
    public RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
        => Physics2D.Raycast(origin, direction, distance, layerMask);

    public RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
        => Physics2D.RaycastAll(origin, direction, distance, layerMask);
}
