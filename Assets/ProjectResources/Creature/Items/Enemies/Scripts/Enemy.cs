using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    public event EventHandler Dead_notifier;
    public event EventHandler GetDamage_notifier;
    public event EventHandler GetHealth_notifier;

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
                Kill();
            }
        }
    }
    public float TimeNoDamage { get => _timeNoDamage; private set => _timeNoDamage = value; }
    public int Damage { get => _damage; set => _damage = value; }

    [Header("Health settings")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _timeNoDamage;

    [Header("Attack settings")]
    [SerializeField] private int _damage;

    [Header("Debug")]
    [SerializeField] private bool _enabledDebugLog;
    [SerializeField] private int _currentHealth;
    [SerializeField] private bool _invulnerability;
    [SerializeField] private bool _isAlive;

    public virtual void GetDamage(int damage)
    {
        if (_isAlive == false)
        {
            return;
        }

        if (_invulnerability)
        {
            //PrintLog($"Enemy {this.gameObject.name} is not damaged because he is invulnerable.");
            return;
        }

        if (damage > 0)
        {
            var previousHealthPoints = CurrentHealth;
            CurrentHealth -= damage;
            GetDamage_notifier?.Invoke(this, EventArgs.Empty);
            StartCoroutine(TimerNoDamge(_timeNoDamage));

            PrintLog($"Enemy {this.gameObject.name} received {damage} damage. Was {previousHealthPoints} health, current {CurrentHealth}, max {MaxHealth}. Removed {previousHealthPoints - CurrentHealth} HP.");
        }
        else
        {
            PrintLog($"Damage < 0. Enemy {this.gameObject.name} is not damaged.");
        }
    }

    public virtual void GetHealth(int health)
    {
        if (health > 0)
        {
            var previousHealthPoints = CurrentHealth;
            CurrentHealth += health;
            GetHealth_notifier?.Invoke(this, EventArgs.Empty);

            PrintLog($"Enemy {this.gameObject.name} received {health} health. Was {previousHealthPoints} health, current {CurrentHealth}, max {MaxHealth}. Added {CurrentHealth - previousHealthPoints} HP.");
        }
    }

    public virtual void TakeDamage(IHealth target)
    {
        target.GetDamage(Damage);
    }

    protected virtual void Kill()
    {
        _isAlive = false;
        Dead_notifier?.Invoke(this, EventArgs.Empty);

        PrintLog($"Enemy {this.gameObject.name} killed.");
    }

    protected virtual IEnumerator TimerNoDamge(float time)
    {
        _invulnerability = true;
        yield return new WaitForSeconds(time);
        _invulnerability = false;
    }

    protected virtual void PrintLog(string text)
    {
        if (_enabledDebugLog)
        {
            Debug.Log(text);
        }
    }
}
