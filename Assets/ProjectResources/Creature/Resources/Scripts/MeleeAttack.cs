using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MeleeAttack : MonoBehaviour, IAttack
{
    public event EventHandler StartedAttack_notifier;
    public event EventHandler FinishedAttack_notifier;
    public event EventHandler Reload_notifier;
    public event EventHandler ReadyToAttack_notifier;

    public List<GameObject> TargetsForDamage_list { get => _targetsForDamage_list; private set => _targetsForDamage_list = value; }
    public List<string> TargetTags_list { get => _targetTags; set => _targetTags = value; }
    public LayerMask TargetLayer { get => _targetLayer; set => _targetLayer = value; }
    public bool IsReadyToAttack { get => _isReadyToAttack; set => _isReadyToAttack = value; }

    [SerializeField] private PlayableDirector _attackTimeline;
    [SerializeField] private int _damage;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _damageDistance;
    [SerializeField] private float _distanceToStartAttack;
    [SerializeField] private List<string> _targetTags;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Debug")]
    [SerializeField] private bool _isReadyToAttack;
    private List<GameObject> _targetsForDamage_list;

    private Coroutine _reloadCoroutine;

    private void Awake()
    {
        IsReadyToAttack = true;
    }

    public void Attack()
    {
        StartedAttack_notifier?.Invoke(this, EventArgs.Empty);
        _attackTimeline.Play();
        _reloadCoroutine = StartCoroutine(TimerReloadAttack(_reloadTime));
    }

    public void FinishAttack()
    {
        FinishedAttack_notifier?.Invoke(this, EventArgs.Empty);
    }

    public bool CheckTargetsInAttackDistance()
    {
        var gameObjects = StandartMethods.GetObjectsInCircleZoneByTagsAndLayerMask(this.transform.position, _distanceToStartAttack, TargetTags_list, TargetLayer);

        if (gameObjects.Count > 0)
        {
            return true;
        }

        return false;
    }

    public void TakeDamageToTargetsInDamageDistance()
    {
        CheckTargetsInDamageDistance();

        if (TargetsForDamage_list == null)
        {
            return;
        }

        foreach (var creature in TargetsForDamage_list)
        {
            if (creature.TryGetComponent<IHealth>(out var health))
            {
                health.GetDamage(_damage);
            }
        }
    }

    private void CheckTargetsInDamageDistance()
    {
        TargetsForDamage_list = StandartMethods.GetObjectsInCircleZoneByTagsAndLayerMask(this.transform.position, _distanceToStartAttack, TargetTags_list, TargetLayer);
    }

    private IEnumerator TimerReloadAttack(float time)
    {
        Reload_notifier?.Invoke(this, EventArgs.Empty);
        _isReadyToAttack = false;
        yield return new WaitForSeconds(time);
        _isReadyToAttack = true;
        ReadyToAttack_notifier?.Invoke(this, EventArgs.Empty);
    }
}