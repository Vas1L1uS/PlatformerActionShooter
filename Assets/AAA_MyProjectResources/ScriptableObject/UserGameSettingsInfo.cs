using UnityEngine;

[CreateAssetMenu(fileName = "UserAudioSettings", menuName = "ScriptableObjects/UserGameSettings/AudioSettings", order = 1)]
public class UserGameSettingsInfo : ScriptableObject
{
    public float MusicVolume => _musicVolume;
    public float SoundVolume => _soundVolume;

    [Header("Debug")]
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _soundVolume;

    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
    }

    public void SetSoundVolume(float value)
    {
        _soundVolume = value;
    }
}
