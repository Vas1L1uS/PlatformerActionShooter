using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _player;
    [SerializeField] private FinishPoint _finishPoint;

    [Header("Load scenes settings")]
    [SerializeField] private string _gameoverSceneName;
    [SerializeField] private string _winSceneName;

    private void Start()
    {
        _player.Dead_notifier += GameOver;
        _finishPoint.PlayerFinished_notifier += LevelComplete;
    }

    private void GameOver(object sender, EventArgs e)
    {
        SceneManager.LoadScene(_gameoverSceneName);
    }

    private void LevelComplete(object sender, EventArgs e)
    {
        SceneManager.LoadScene(_winSceneName);
    }
}
