using UnityEngine;

public class DevToolPanel : MonoBehaviour
{
    [SerializeField] private Canvas _dev_canvas;

    [Header("Player")]
    [SerializeField] private PlayerHealth _playerHealth;

    private DevActions _input;

    private void Awake()
    {
        _input = new DevActions();
        _input.Devel.DevPanel.performed += context => PressButtonForOpenDevPanel();
    }

    public void OpenDevPanel()
    {
        _dev_canvas.gameObject.SetActive(true);
    }
    public void CloseDevPanel()
    {
        _dev_canvas.gameObject.SetActive(false);
    }
    private void PressButtonForOpenDevPanel()
    {
        if (_dev_canvas.isActiveAndEnabled)
        {
            CloseDevPanel();
        }
        else
        {
            OpenDevPanel();
        }
    }

    #region DebugTestMethods

    public void AddPlayerHealth()
    {
        _playerHealth.GetHealth(1);
    }
    public void RemovePlayerHealth()
    {
        _playerHealth.GetDamage(1);
    }

    #endregion

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}