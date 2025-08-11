using UnityEngine;

public interface IAimProvider
{
    // 원점과 조준 방향을 계산해 반환
    void GetAim(out Vector2 origin, out Vector2 direction);
}

public class MouseAimProvider : IAimProvider
{
    readonly Transform muzzle;
    readonly Camera camera;
    readonly Transform owner;

    public MouseAimProvider(Transform muzzle, Camera camera, Transform owner)
    {
        this.muzzle = muzzle;
        this.camera = camera;
        this.owner = owner;
    }

    public void GetAim(out Vector2 origin, out Vector2 direction)
    {
        origin = muzzle ? (Vector2)muzzle.position : (Vector2)owner.position;
        var cam = camera ? camera : Camera.main;
        direction = Vector2.right;
        if (!cam)
        {
            direction = Vector2.zero;
            return;
        }

        Vector3 mouse = Input.mousePosition;
        float planeZ = owner.position.z;
        mouse.z = planeZ - cam.transform.position.z;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouse);

        Vector2 toMouse = (Vector2)mouseWorld - origin;
        float dist = toMouse.magnitude;
        if (dist <= 0.0001f) { direction = Vector2.zero; return; }
        direction = toMouse / Mathf.Max(0.0001f, dist);
    }
}
