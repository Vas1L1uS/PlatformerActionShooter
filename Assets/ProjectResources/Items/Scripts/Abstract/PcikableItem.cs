using System.Collections;
using UnityEngine;

public abstract class PickableItem : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private float _timeToDestroy = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _name_destroyClip = "Collected";
    [SerializeField] private AudioSource _audioSource;

    private bool _isCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected)
        {
            return;
        }

        if (collision.CompareTag(_playerTag))
        {
            CheckAndPick(collision.gameObject);
        }
    }

    protected virtual void CheckAndPick(GameObject player)
    {
        Collected();
    }

    protected virtual void Collected()
    {
        _isCollected = true;
        _animator.Play(_name_destroyClip);
        _audioSource.Play();
        StartCoroutine(TimerToDestroy(_timeToDestroy));
    }

    private IEnumerator TimerToDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}