using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CreatureHealth, IHaveInvulnerability
{
    public override event EventHandler Dead_notifier;
    public override event EventHandler GetDamage_notifier;
    public override event EventHandler GetHealth_notifier;

    public bool IsInvulnerability { get => _IsInvulnerability; set => _IsInvulnerability = value; }
    public float TimeNoDamage { get => _timeNoDamage; set => _timeNoDamage = value; }

    [Header ("PlayerHealth settings")]
    [SerializeField] private bool _IsInvulnerability;
    [SerializeField] private float _timeNoDamage;

    public IEnumerator TimerNoDamge(float time)
    {
        throw new NotImplementedException();
    }

    public override void GetDamage(int damage)
    {
        if (IsAlive == false)
        {
            return;
        }

        if (IsInvulnerability)
        {
            //PrintLogInEditor($"{this.gameObject.name} is not damaged because he is invulnerable.");
            return;
        }

        if (damage > 0)
        {
            var previousHealthPoints = CurrentHealth;
            CurrentHealth -= damage;
            GetDamage_notifier?.Invoke(this, EventArgs.Empty);
            StartCoroutine(TimerNoDamge(TimeNoDamage));

            PrintLogInEditor($"{this.gameObject.name} received {damage} damage. Was {previousHealthPoints} health, current {CurrentHealth}, max {MaxHealth}. Removed {previousHealthPoints - CurrentHealth} HP.");
        }
        else
        {
            PrintLogInEditor($"Damage < 0. {this.gameObject.name} is not damaged.");
        }
    }
}