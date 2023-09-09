using System;
using TMPro;
using UnityEngine;

public class ScorePanelController : MonoBehaviour
{
    [SerializeField] private TMP_Text _totalScore;
    [SerializeField] private TMP_Text _enemiesKilled;
    [SerializeField] private TMP_Text _damageReceived;
    [SerializeField] private TMP_Text _shotsFired;
    [SerializeField] private TMP_Text _timePassed;

    [SerializeField] private GamestatsInfo _gamestats;

    private void Awake()
    {
        _gamestats.CountPoints();
        _totalScore.text = _gamestats.TotalScore.ToString();
        _enemiesKilled.text = _gamestats.EnemiesKilled.ToString();
        _damageReceived.text = _gamestats.DamageReceived.ToString();
        _shotsFired.text = _gamestats.ShotsFired.ToString();
        _timePassed.text = GetTimeMinSecFormat(_gamestats.SecondsPassed);
    }

    private string GetTimeMinSecFormat(int value)
    {
        int min;
        int sec;

        sec = value % 60;
        min = (int)Math.Floor((decimal)(value / 60));

        if (sec < 10)
        {
            return $"{min}:0{sec}";
        }
        else
        {
            return $"{min}:{sec}";
        }
    }
}