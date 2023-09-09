using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarValue : MonoBehaviour
{
    public event EventHandler ValueChanged_notifier;

    public float Value => _value;
    private float _value;

    [SerializeField] private Scrollbar _scrollbar;

    public void ChangeValue()
    {
        _value = _scrollbar.value;
        ValueChanged_notifier?.Invoke(this, EventArgs.Empty);
    }
}