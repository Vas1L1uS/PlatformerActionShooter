using UnityEngine;

public class Saw : MonoBehaviour, IDamager
{
    public int Damage { get => _damage; set => _damage = value; }

    [SerializeField] private int _damage;
    [SerializeField] private float _damageRadius;
    [SerializeField] private LayerMask _damageableMask;

    private void Update()
    {
        var hits = Physics2D.OverlapCircleAll(this.transform.position, _damageRadius, _damageableMask);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.gameObject.TryGetComponent<IHealth>(out var health))
                {
                    TakeDamage(health);
                }
            }
        }
    }

    public void TakeDamage(IHealth target)
    {
        target.GetDamage(Damage);
    }
}
