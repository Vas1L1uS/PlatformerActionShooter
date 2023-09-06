using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHealth : MonoBehaviour, IHealth, IDebugLogger
{
    public virtual event EventHandler Dead_notifier;
    public virtual event EventHandler<IntValueEventArgs> GetDamage_notifier;
    public virtual event EventHandler<IntValueEventArgs> GetHealth_notifier;

    /// <summary>
    /// Only read
    /// </summary>
    public bool IsAlive { get => _isAlive; set => _isAlive = true; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;

            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
            else if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
        }
    }
    public List<GameobjectLog> GameobjectLog_list {get => _gameobjectLog_list; set => _gameobjectLog_list = value; }
    public bool EnabledPrintDebugLogInEditor { get => _enabledPrintDebugLogInEditor; set => _enabledPrintDebugLogInEditor = value; }
    public bool EnabledAddingLogs { get => _enabledAddingLogs; set => _enabledAddingLogs = value; }

    [SerializeField] private int _maxHealth;
    [SerializeField] private GameObject _DieParticle;

    [Header("Debug")]
    [SerializeField] private bool _enabledPrintDebugLogInEditor;
    [SerializeField] private bool _enabledAddingLogs;
    [SerializeField] private int _currentHealth;
    [SerializeField] private bool _isAlive;

    private List<GameobjectLog> _gameobjectLog_list;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        _isAlive = true;
        _gameobjectLog_list = new List<GameobjectLog>();
    }

    public virtual void GetDamage(int damage)
    {
        if (IsAlive == false)
        {
            return;
        }

        if (damage > 0)
        {
            var previousHealthPoints = CurrentHealth;
            CurrentHealth -= damage;

            GetDamage_notifier?.Invoke(this, new IntValueEventArgs() { Value = damage });

            PrintLogInEditor($"{this.gameObject.name} received {damage} damage. Was {previousHealthPoints} health, current {CurrentHealth}, max {MaxHealth}. Removed {previousHealthPoints - CurrentHealth} HP.");
        }
        else
        {
            PrintLogInEditor($"Damage < 0. {this.gameObject.name} is not damaged.");
        }
    }

    public virtual void GetHealth(int health)
    {
        if (health > 0)
        {
            var previousHealthPoints = CurrentHealth;
            CurrentHealth += health;
            GetHealth_notifier?.Invoke(this, new IntValueEventArgs() { Value = health });

            PrintLogInEditor($"{this.gameObject.name} received {health} health. Was {previousHealthPoints} health, current {CurrentHealth}, max {MaxHealth}. Added {CurrentHealth - previousHealthPoints} HP.");
        }
    }

    public virtual void Die()
    {
        _isAlive = false;
        Dead_notifier?.Invoke(this, EventArgs.Empty);
        Instantiate(_DieParticle, this.transform.position, Quaternion.identity);

        PrintLogInEditor($"{this.gameObject.name} killed.");

        Destroy(this.gameObject);
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