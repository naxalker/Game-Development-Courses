using System;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable, ITickable, IDisposable
{
    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    public event Action<State> StateChanged;
    public event Action GamePaused;
    public event Action GameUnpaused;

    private const float GamePlayingTimerMax = 120f;

    private readonly GameInput _gameInput;

    private State _state;
    private bool _gameIsPaused;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;

    public bool IsGamePlaying => _state == State.GamePlaying;
    public float CountdownToStartTimer => _countdownToStartTimer;
    public float GamePlayingTimerNormalized => 1f - _gamePlayingTimer / GamePlayingTimerMax;

    public GameManager(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    public void Initialize()
    {
        _state = State.WaitingToStart;

        _gameInput.PausePressed += PausePressedHandler;
        _gameInput.InteractPressed += InteractPressedHandler;
    }

    public void Dispose()
    {
        _gameInput.PausePressed -= PausePressedHandler;
        _gameInput.InteractPressed -= InteractPressedHandler;
    }

    public void Tick()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0f)
                {
                    _state = State.GamePlaying;

                    _gamePlayingTimer = GamePlayingTimerMax;

                    StateChanged?.Invoke(_state);
                }
                break;

            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0f)
                {
                    _state = State.GameOver;

                    StateChanged?.Invoke(_state);
                }
                break;

            case State.GameOver:
                break;

        }
    }

    public void TogglePause()
    {
        if (_gameIsPaused)
        {
            Time.timeScale = 1;
            GameUnpaused?.Invoke();
        }
        else
        {
            Time.timeScale = 0;
            GamePaused?.Invoke();
        }

        _gameIsPaused = !_gameIsPaused;
    }

    private void InteractPressedHandler()
    {
        if (_state == State.WaitingToStart)
        {
            _state = State.CountdownToStart;
            StateChanged?.Invoke(_state);
        }
    }

    private void PausePressedHandler()
    {
        TogglePause();
    }
}
