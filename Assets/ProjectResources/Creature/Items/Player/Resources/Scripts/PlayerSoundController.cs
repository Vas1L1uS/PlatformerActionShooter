using System;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [Header("Shoot sound settings")]
    [SerializeField] private AudioSource _shoot_AudioSource;
    [SerializeField] private AudioClip _shoot;

    [Header("Health sound settings")]
    [SerializeField] private IHealth _playerHealth;
    [SerializeField] private AudioSource _health_AudioSource;
    [SerializeField] private AudioClip _damage;

    private void Awake()
    {
        _playerController.Shoot_notifier += PlaySoundShoot;
        _playerHealth.GetDamage_notifier += PlaySoundAddHealth;
    }

    private void PlaySoundShoot(object sender = null, EventArgs e = null)
    {
        _shoot_AudioSource.clip = _shoot;
        _shoot_AudioSource.Play();
    }

    private void PlaySoundAddHealth(object sender = null, EventArgs e = null)
    {
        _health_AudioSource.clip = _damage;
        _health_AudioSource.Play();
    }
}