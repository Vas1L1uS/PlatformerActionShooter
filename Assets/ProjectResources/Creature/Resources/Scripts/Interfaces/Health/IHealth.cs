using System;

public interface IHealth
{
    event EventHandler Dead_notifier;
    event EventHandler<IntValueEventArgs> GetDamage_notifier;
    event EventHandler<IntValueEventArgs> GetHealth_notifier;
    bool IsAlive { get; set; }
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    void GetDamage(int damage);
    void GetHealth(int health);
    void Die();
}