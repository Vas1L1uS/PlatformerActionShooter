using System;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public event EventHandler PlayerFinished_notifier;

    [SerializeField] private string _playerTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            PlayerFinished_notifier?.Invoke(this, EventArgs.Empty);
        }
    }
}
