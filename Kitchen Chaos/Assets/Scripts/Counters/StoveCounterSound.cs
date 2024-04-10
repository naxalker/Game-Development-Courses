using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StoveCounterSound : MonoBehaviour
{
    private StoveCounter _stoveCounter;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _stoveCounter = GetComponentInParent<StoveCounter>();
    }

    private void OnEnable()
    {
        _stoveCounter.StateChanged += StateChangedHandler;
    }

    private void OnDisable()
    {
        _stoveCounter.StateChanged -= StateChangedHandler;
    }

    private void StateChangedHandler(StoveCounter.State state)
    {
        if (state == StoveCounter.State.Frying || state == StoveCounter.State.Fried)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }
}
