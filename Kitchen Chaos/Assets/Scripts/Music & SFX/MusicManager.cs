using UnityEngine;

public class MusicManager
{
    private AudioSource _backgroundMusic;
    private float _volume = .3f;

    public MusicManager(AudioSource backgroundMusic)
    {
        _backgroundMusic = backgroundMusic;
    }

    public float Volume => _volume;

    public void ChangeVolume()
    {
        _volume += .1f;

        if (_volume > 1f)
        {
            _volume = 0f;
        }

        _backgroundMusic.volume = _volume;
    }
}
