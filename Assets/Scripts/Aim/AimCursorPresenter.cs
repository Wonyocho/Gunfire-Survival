using UnityEngine;

/// <summary>
/// 조준 커서를 화면상의 마우스 월드 좌표로 이동.
/// </summary>
public class AimCursorPresenter : MonoBehaviour
{
    [SerializeField] Transform targetCursor;
    [SerializeField] Transform owner;
    [SerializeField] Camera cam;

    void Awake()
    {
        if (!owner) owner = transform;
        if (!cam) cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!targetCursor || !cam) return;
        Vector3 mouse = Input.mousePosition;
        float planeZ = owner.position.z;
        mouse.z = planeZ - cam.transform.position.z;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouse);
        targetCursor.position = new Vector3(mouseWorld.x, mouseWorld.y, planeZ);
    }
}
