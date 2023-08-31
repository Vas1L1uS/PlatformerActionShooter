using System;

public interface IHealth
{
    event EventHandler Dead_notifier;
    event EventHandler GetDamage_notifier;
    event EventHandler GetHealth_notifier;
    bool IsAlive { get; set; }
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
    void GetDamage(int damage);
    void GetHealth(int health);
}