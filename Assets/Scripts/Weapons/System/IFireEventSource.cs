using UnityEngine;

/// <summary>
/// 발사 이벤트를 외부에 노출하는 소스(무기 발사 컨트롤러 등).
/// </summary>
public interface IFireEventSource
{
    event System.Action<Vector2, Vector2, IWeapon> OnFired;
}
