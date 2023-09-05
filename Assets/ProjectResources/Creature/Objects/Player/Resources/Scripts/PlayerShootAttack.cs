using System;
using UnityEngine;

public class PlayerShootAttack : ShootAttack
{
    public override event EventHandler StartedAttack_notifier;

    public override void Attack()
    {
        Shoot(TargetPosition);
    }

    public override void Shoot(Vector2 targetPosition)
    {
        if (_bulletLeft <= 0)
        {
            return;
        }

        GameObject newBullet = Instantiate(_bullet_prefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = targetPosition * _bulletSpeed;
        _bulletLeft--;

        StartedAttack_notifier?.Invoke(this, EventArgs.Empty);

        if (_bulletLeft <= 0)
        {
            IsReadyToAttack = false;
            _reloadCoroutine = StartCoroutine(ShootReload(_reloadTime));
        }
    }
}
