using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private List<Image> _ammo_images_list;
    [SerializeField] private float _flickerFrequency;

    private bool _reload;
    private Color _image_startColor;
    private Coroutine _reload_coroutine;

    private void Awake()
    {
        _image_startColor = _ammo_images_list[1].color;
        EnableImagesMaxAmmo(_ammo_images_list);
        _playerController.Shoot_notifier += RemoveBullet;
        _playerController.Reload_notifier += Reload;
        _playerController.ReadyToShoot_notifier += ReadyToShoot;
    }

    private void EnableImagesMaxAmmo(List<Image> images)
    {
        foreach (Image image in images)
        {
            image.enabled = false;
        }

        for (int i = 0; i < _playerController.MaxStockBullet; i++)
        {
            images[i].enabled = true;
        }
    }

    private void RemoveBullet(object sender, EventArgs e)
    {
        for (int i = _playerController.MaxStockBullet - 1; i >= _playerController.BulletLeft; i--)
        {
            _ammo_images_list[i].color = new Color(0, 0, 0, 1);
        }
    }

    private void Reload(object sender, EventArgs e)
    {
        _reload = true;
        _reload_coroutine = StartCoroutine(ReloadTimer(_flickerFrequency));
    }

    private void ReadyToShoot(object sender, EventArgs e)
    {
        _reload = false;
        StopCoroutine(_reload_coroutine);

        for (int i = 0; i < _ammo_images_list.Count; i++)
        {
            _ammo_images_list[i].color = _image_startColor;
        }
    }

    private IEnumerator ReloadTimer(float time)
    {
        while (_reload)
        {
            yield return new WaitForSeconds(time);

            for (int i = 0; i < _ammo_images_list.Count; i++)
            {
                _ammo_images_list[i].color = _image_startColor / 1.5f;
            }

            yield return new WaitForSeconds(time);

            for (int i = 0; i < _ammo_images_list.Count; i++)
            {
                _ammo_images_list[i].color = new Color(0, 0, 0, 1);
            }
        }
    }
}
