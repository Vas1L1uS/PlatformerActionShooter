using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShootAttack : MonoBehaviour, IAttack, IDebugLogger
{
    public event EventHandler StartedAttack_notifier;
    public event EventHandler FinishedAttack_notifier;
    public event EventHandler Reload_notifier;
    public event EventHandler ReadyToAttack_notifier;

    public List<GameobjectLog> GameobjectLog_list { get => _gameobjectLog_list; set => _gameobjectLog_list = value; }
    public bool EnabledPrintDebugLogInEditor { get => _enabledPrintDebugLogInEditor; set => _enabledPrintDebugLogInEditor = value; }
    public bool EnabledAddingLogs { get => _enabledAddingLogs; set => _enabledAddingLogs = value; }
    public List<string> TargetTags_list { get => _targetTags; set => _targetTags = value; }
    public LayerMask TargetLayer { get => _targetLayer; set => _targetLayer = value; }
    public bool LeftShootDirection { get => _leftShootDirection; set => _leftShootDirection = value; }
    public int MaxStockBullet => _maxStockBullet;
    public int BulletLeft => _bulletLeft;
    public bool IsReadyToAttack { get; set; }

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _bullet_prefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _maxStockBullet;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _distanceToStartAttack;
    [SerializeField] private List<string> _targetTags;
    [SerializeField] private LayerMask _targetLayer;

    [Header("Debug")]
    [SerializeField] private bool _enabledPrintDebugLogInEditor;
    [SerializeField] private bool _enabledAddingLogs;
    [SerializeField] private int _bulletLeft;
    [SerializeField] private bool _leftShootDirection;

    private List<GameobjectLog> _gameobjectLog_list;
    private Coroutine _reloadCoroutine;

    private void Awake()
    {
        _gameobjectLog_list = new List<GameobjectLog>();
        _bulletLeft = MaxStockBullet;
        IsReadyToAttack = true;
    }

    public void Attack()
    {
        Shoot(LeftShootDirection);
    }

    public void FinishAttack()
    {
        FinishedAttack_notifier?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot(bool leftDirection)
    {
        if (_bulletLeft <= 0)
        {
            return;
        }

        float direction = 1;

        if (leftDirection)
        {
            direction = -1;
        }

        GameObject newBullet = Instantiate(_bullet_prefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector3(_bulletSpeed * direction, 0, 0);
        _bulletLeft--;

        IsReadyToAttack = false;

        StartedAttack_notifier?.Invoke(this, EventArgs.Empty);
        FinishedAttack_notifier?.Invoke(this, EventArgs.Empty);

        if (_bulletLeft <= 0)
        {
            _reloadCoroutine = StartCoroutine(ShootReload(_reloadTime));
        }
        else
        {
            IsReadyToAttack = true;
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

    private IEnumerator ShootReload(float reloadTime)
    {
        Reload_notifier?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(reloadTime);
        _bulletLeft = _maxStockBullet;
        ReadyToAttack_notifier?.Invoke(this, EventArgs.Empty);
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