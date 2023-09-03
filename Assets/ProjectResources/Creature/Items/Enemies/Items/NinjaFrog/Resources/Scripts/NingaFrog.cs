using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NingaFrog : Enemy
{
    public float ReloadTime { get => _reloadTime; set => _reloadTime = value; }

    [Header("NingaFrog settings")]
    [Header("Attack settings")]
    [SerializeField] private PlayableDirector _attackTimeline;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _damageDistance;
    [SerializeField] private float _distanceToStartAttack;

    [Header("Vision settings")]
    [SerializeField] private string _playerTag;
    [SerializeField] private LayerMask _creatureLayer;
    [SerializeField] private float _visionDistance;

    [Header("Movement settings")]
    [SerializeField] private GameObject _leftStopPoint;
    [SerializeField] private GameObject _rightStopPoint;

    [Header("Animation settings")]
    [SerializeField] private Animator _animator;

    [Header("Debug")]
    [SerializeField] private bool _isReadyToAttack;
    private GameObject[] _creaturesForDamage;

    private void Awake()
    {
        BaseAwake();
        _isReadyToAttack = true;
    }

    private void Update()
    {
        if (CheckPlayerInVision())
        {
            if (_isReadyToAttack)
            {
                if (CheckPlayerInAttackDistance())
                {
                    Attack();
                    TimerReloadAttack(_reloadTime);
                }
                else
                {
                    _animator.Play("Run");
                }
            }
            else
            {
                _animator.Play("Run");
            }
        }
    }

    public void TakeDamageToCreautesInDamageZone()
    {
        foreach (var creature in _creaturesForDamage)
        {
            if (creature.TryGetComponent<IHealth>(out var health))
            {
                health.GetDamage(base.Damage);
            }
        }
    }

    private bool CheckPlayerInAttackDistance()
    {
        RaycastHit2D[] attackCreatures = Physics2D.CircleCastAll(this.transform.position, _distanceToStartAttack, Vector2.zero, _creatureLayer);

        _creaturesForDamage = GetHitsTag(_playerTag, attackCreatures);

        if (_creaturesForDamage.Length > 0)
        {
            return true;
        }

        return false;
    }

    private void Attack()
    {
        _attackTimeline.Play();
    }

    private IEnumerator TimerReloadAttack(float time)
    {
        _isReadyToAttack = false;
        yield return new WaitForSeconds(time);
        _isReadyToAttack = true;
    }
}