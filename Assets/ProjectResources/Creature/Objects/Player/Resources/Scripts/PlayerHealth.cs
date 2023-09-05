using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CreatureHealth, IHaveInvulnerability
{
    public override event EventHandler GetDamage_notifier;

    public bool IsInvulnerability { get => _isInvulnerability; set => _isInvulnerability = value; }
    public float TimeNoDamage { get => _timeNoDamage; set => _timeNoDamage = value; }

    [Header ("PlayerHealth settings")]
    [SerializeField] private bool _isInvulnerability;
    [SerializeField] private float _timeNoDamage;

    public IEnumerator TimerNoDamge(float time)
    {
        _isInvulnerability = true;
        yield return new WaitForSeconds(time);
        _isInvulnerability = false;
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