using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void LoadSceneByName()
    {
        SceneManager.LoadScene(_sceneName);
    }
}