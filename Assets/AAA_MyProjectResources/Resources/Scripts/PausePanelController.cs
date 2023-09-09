using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelController : MonoBehaviour
{
    public bool IsPaused => _isPaused;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private string _menuSceneName;

    [Header("Debug")]
    [SerializeField] private bool _isPaused;

    private PauseInput _input;

    private void Awake()
    {
        _input = new PauseInput();
        _input.Newactionmap.Newaction.performed += context => PausePlay();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void Continue()
    {
        _pausePanel.SetActive(false);
        _isPaused = false;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        _pausePanel.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0;
    }

    public void PausePlay()
    {
        if (_isPaused)
        {
            Continue();
        }
        else
        {
            Pause();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_menuSceneName);
    }
}
