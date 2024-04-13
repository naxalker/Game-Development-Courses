using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class StoveCounterSound : MonoBehaviour
{
    private const float BurnShowProgressAmount = .5f;
    private const float WarningSoundTimerMax = .2f;

    private SoundManager _soundManager;
    private StoveCounter _stoveCounter;
    private AudioSource _audioSource;

    private float _warningSoundTimer;
    private bool _playWarningSound;

    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _stoveCounter = GetComponentInParent<StoveCounter>();
    }

    private void OnEnable()
    {
        _stoveCounter.StateChanged += StateChangedHandler;
        _stoveCounter.ProgressChanged += ProgressChangesHandler;
    }

    private void OnDisable()
    {
        _stoveCounter.StateChanged -= StateChangedHandler;
        _stoveCounter.ProgressChanged -= ProgressChangesHandler;
    }

    private void Update()
    {
        if (_playWarningSound)
        {
            _warningSoundTimer -= Time.deltaTime;

            if (_warningSoundTimer <= 0)
            {
                _warningSoundTimer = WarningSoundTimerMax;

                _soundManager.PlayWarningSound(_stoveCounter.transform.position);
            }
        }
    }

    private void ProgressChangesHandler(float progress)
    {
        _playWarningSound = _stoveCounter.IsFried && progress >= BurnShowProgressAmount;
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
