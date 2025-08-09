using UnityEngine;

public class EnemyRegister : MonoBehaviour
{
    EnemyHealth eh;

    void Awake()
    {
        eh = GetComponent<EnemyHealth>();
        if (!eh) eh = gameObject.AddComponent<EnemyHealth>();
    }

    void OnEnable()
    {
        EnemyRegistry.All.Add(eh);
    }

    void OnDisable()
    {
        EnemyRegistry.All.Remove(eh);
    }
}
