using System;
using System.Collections.Generic;
using UnityEngine;

public class GamestatsController : MonoBehaviour
{
    [SerializeField] private List<CreatureHealth> _enemiesHealth;
    [SerializeField] private CreatureHealth _playerHealth;
    [SerializeField] private PlayerShootAttack _playerShootAttack;
    [SerializeField] private GamestatsInfo _gamestatsInfo;

    private void Start()
    {
        _gamestatsInfo.ResetStats();

        SubscribeToDeathEnemies(_enemiesHealth);
        _playerHealth.GetDamage_notifier += AddDamageReceived;
        _playerShootAttack.StartedAttack_notifier += AddShot;
    }

    private void SubscribeToDeathEnemies(List<CreatureHealth> enemiesHealth)
    {
        for (int i = 0; i < enemiesHealth.Count; i++)
        {
            enemiesHealth[i].Dead_notifier += AddEnemyKill;
        }
    }

    private void AddEnemyKill(object sender, EventArgs e)
    {
        _gamestatsInfo.AddEnemyKill();
    }

    private void AddDamageReceived(object sender, EventArgs e)
    {
        var eventArgs = e as IntValueEventArgs;
        _gamestatsInfo.AddDamageReceived(eventArgs.Value);
    }

    private void AddShot(object sender, EventArgs e)
    {
        _gamestatsInfo.AddShot();
    }
}