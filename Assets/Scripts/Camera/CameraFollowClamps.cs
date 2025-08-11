using UnityEngine;

public class CameraFollowSmoothClamp : MonoBehaviour
{
    [SerializeField] Transform target;           // Player
    [SerializeField] BoxCollider2D worldBounds;  // 큰 맵 경계
    [SerializeField] float smoothTime = 0.12f;   // 0.08~0.2 추천

    Camera cam;
    Vector3 velocity; // SmoothDamp용 내부 상태

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam) cam = Camera.main;
        if (cam) cam.orthographic = true;
    }

    void LateUpdate()
    {
        if (!target || !cam) return;

        // 1) 먼저 스무딩으로 타겟을 따라가고
        Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 smoothed = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);

        // 2) 그 다음 카메라 크기 고려해서 경계 클램프
        if (worldBounds)
        {
            var b = worldBounds.bounds;
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            smoothed.x = Mathf.Clamp(smoothed.x, b.min.x + halfW, b.max.x - halfW);
            smoothed.y = Mathf.Clamp(smoothed.y, b.min.y + halfH, b.max.y - halfH);
        }

        transform.position = smoothed;
    }
}
