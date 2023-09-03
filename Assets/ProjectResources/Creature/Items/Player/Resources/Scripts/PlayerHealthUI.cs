using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private IHealth _playerHealth;
    [SerializeField] private List<Image> _health_images_list;

    private void Awake()
    {
        EnableImagesMaxHealth(_health_images_list);
        _playerHealth.GetDamage_notifier += RemoveHealth;
        _playerHealth.GetHealth_notifier += AddHealth;
    }

    private void EnableImagesMaxHealth(List<Image> images)
    {
        foreach (Image image in images)
        {
            image.enabled = false;
        }

        for (int i = 0; i < _playerHealth.MaxHealth; i++)
        {
            images[i].enabled = true;
        }
    }

    private void AddHealth(object sender, EventArgs e)
    {
        for (int i = 0; i < _playerHealth.CurrentHealth; i++)
        {
            _health_images_list[i].color = new Color(1, 1, 1, 1);
        }
    }

    private void RemoveHealth(object sender, EventArgs e)
    {
        for (int i = _playerHealth.MaxHealth - 1; i >= _playerHealth.CurrentHealth; i--)
        {
            _health_images_list[i].color = new Color(0, 0, 0, 1);
        }
    }
}
