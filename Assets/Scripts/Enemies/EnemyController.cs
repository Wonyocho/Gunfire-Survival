using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageSource
{
    [SerializeField] private float damage = 10f;

    public float Damage => damage;
}
