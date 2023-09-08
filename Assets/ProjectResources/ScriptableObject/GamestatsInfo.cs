using UnityEngine;

[CreateAssetMenu(fileName = "GameStats", menuName = "ScriptableObjects/GamestatsInfo", order = 1)]
public class GamestatsInfo : ScriptableObject
{
    public int TotalPoints => _totalPoints;
    public int EnemiesKilled => _enemiesKilled;
    public int DamageReceived => _damageReceived;
    public int ShotsFired => _shotsFired;
    public int SecondsPassed => _secondsPassed;

    [SerializeField] private int _totalPoints = 0;
    [SerializeField] private int _enemiesKilled = 0;
    [SerializeField] private int _damageReceived = 0;
    [SerializeField] private int _shotsFired = 0;
    [SerializeField] private int _secondsPassed = 0;

    public void CountPoints()
    {
        _totalPoints = 500 - (_enemiesKilled * 15) - (_damageReceived * 3) - _shotsFired - _secondsPassed;
    }

    public void AddEnemyKill()
    {
        _enemiesKilled++;
    }

    public void AddDamageReceived(int damage)
    {
        _damageReceived += damage;
    }

    public void AddShot()
    {
        _shotsFired++;
    }

    public void ResetStats()
    {
        _totalPoints = 0;
        _enemiesKilled = 0;
        _damageReceived = 0;
        _shotsFired = 0;
    }
}