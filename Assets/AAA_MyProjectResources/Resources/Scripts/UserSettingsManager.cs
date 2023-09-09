using System;
using UnityEngine;
using UnityEngine.Audio;

public class UserSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private ScrollbarValue _musicScrollbar;
    [SerializeField] private ScrollbarValue _soundScrollbar;

    private void Start()
    {
        _musicScrollbar.ValueChanged_notifier += SetMusicVolume;
        _soundScrollbar.ValueChanged_notifier += SetSoundVolume;
    }

    private void SetMusicVolume(object sender, EventArgs e)
    {
        var volume = -80 + _musicScrollbar.Value * 80;
        _audioMixer.SetFloat("Music", volume);
    }

    private void SetSoundVolume(object sender, EventArgs e)
    {
        var volume = -80 + _soundScrollbar.Value * 80;
        _audioMixer.SetFloat("Sound", volume);
    }
}