using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IDamager
{
    public int Damage { get => _damage; set => _damage = value; }

    [SerializeField] private ParticleSystem _trackParticleSystem;
    [SerializeField] private ParticleSystem _boomParticleSystem;
    [SerializeField] private int _damage;
    [SerializeField] private float _timerToDestroy;
    [SerializeField] private GameObject _render;
    [SerializeField] private LayerMask _collisionMask;

    private Collider2D _myCollider;
    private Rigidbody2D _myRB;


    private void Awake()
    {
        _myCollider = this.GetComponent<Collider2D>();
        _myRB = this.GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(IHealth target)
    {
        target.GetDamage(Damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StandartMethods.CheckBitInNumber(collision.gameObject.layer, _collisionMask) == false)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            return;
        }


        Instantiate(_boomParticleSystem, this.transform.position, Quaternion.identity);

        if (collision.TryGetComponent<IHealth>(out var target))
        {
            target.GetDamage(Damage);
        }

        StartCoroutine(TimerToDestroy(_timerToDestroy));
    }

    private IEnumerator TimerToDestroy(float timerToDestroy)
    {
        Destroy(_render);
        _myCollider.enabled = false;
        _myRB.simulated = false;
        _trackParticleSystem.Stop();
        yield return new WaitForSeconds(timerToDestroy);
        Destroy(this.gameObject);
    }
}