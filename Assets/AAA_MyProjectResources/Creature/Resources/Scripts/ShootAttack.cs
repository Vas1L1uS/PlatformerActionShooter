using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : MonoBehaviour, IAttack, IDebugLogger
{
    public virtual event EventHandler StartedAttack_notifier;
    public virtual event EventHandler FinishedAttack_notifier;
    public virtual event EventHandler Reload_notifier;
    public virtual event EventHandler ReadyToAttack_notifier;

    public List<GameobjectLog> GameobjectLog_list { get => _gameobjectLog_list; set => _gameobjectLog_list = value; }
    public bool EnabledPrintDebugLogInEditor { get => _enabledPrintDebugLogInEditor; set => _enabledPrintDebugLogInEditor = value; }
    public bool EnabledAddingLogs { get => _enabledAddingLogs; set => _enabledAddingLogs = value; }
    public List<string> TargetTags_list { get => _targetTags; set => _targetTags = value; }
    public LayerMask TargetLayer { get => _targetLayer; set => _targetLayer = value; }
    public Vector2 TargetPosition { get => _targetPosition; set => _targetPosition = value; }
    public int MaxStockBullet => _maxStockBullet;
    public int BulletLeft => _bulletLeft;
    public bool IsReadyToAttack { get; set; }

    [SerializeField] protected Transform _bulletSpawnPoint;
    [SerializeField] protected GameObject _bullet_prefab;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected int _maxStockBullet;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected float _distanceToStartAttack;
    [SerializeField] protected List<string> _targetTags;
    [SerializeField] protected LayerMask _targetLayer;

    [Header("Debug")]
    [SerializeField] protected bool _enabledPrintDebugLogInEditor;
    [SerializeField] protected bool _enabledAddingLogs;
    [SerializeField] protected int _bulletLeft;
    [SerializeField] protected Vector2 _targetPosition;

    protected List<GameobjectLog> _gameobjectLog_list;
    protected Coroutine _reloadCoroutine;

    private void Awake()
    {
        _gameobjectLog_list = new List<GameobjectLog>();
        _bulletLeft = MaxStockBullet;
        IsReadyToAttack = true;
    }

    public virtual void Attack()
    {
        Shoot(TargetPosition);
    }

    public void FinishAttack()
    {
        FinishedAttack_notifier?.Invoke(this, EventArgs.Empty);
    }

    public virtual void Shoot(Vector2 targetPosition)
    {
        if (_bulletLeft <= 0)
        {
            return;
        }

        var myPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        var bulletDirection = (targetPosition - myPosition).normalized;

        GameObject newBullet = Instantiate(_bullet_prefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * _bulletSpeed;
        _bulletLeft--;

        StartedAttack_notifier?.Invoke(this, EventArgs.Empty);

        if (_bulletLeft <= 0)
        {
            IsReadyToAttack = false;
            _reloadCoroutine = StartCoroutine(ShootReload(_reloadTime));
        }
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

    protected IEnumerator ShootReload(float reloadTime)
    {
        Reload_notifier?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(reloadTime);
        _bulletLeft = _maxStockBullet;
        ReadyToAttack_notifier?.Invoke(this, EventArgs.Empty);
        FinishedAttack_notifier?.Invoke(this, EventArgs.Empty);
        IsReadyToAttack = true;
    }

    public void PrintLogInEditor(string text)
    {
        if (EnabledPrintDebugLogInEditor)
        {
            Debug.Log(text);
        }
    }

    public void AddLog(GameobjectLog log)
    {
        if (EnabledAddingLogs)
        {
            GameobjectLog_list.Add(log);
        }
    }
}