using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    public float Damage => damage;
}
