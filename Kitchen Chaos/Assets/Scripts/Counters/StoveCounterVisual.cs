using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject _stoveOn;
    [SerializeField] private GameObject _particles;

    private StoveCounter _counter;

    private void Awake()
    {
        _counter = GetComponentInParent<StoveCounter>();
    }

    private void OnEnable()
    {
        _counter.StateChanged += StateChangedHandler;
    }

    private void OnDisable()
    {
        _counter.StateChanged -= StateChangedHandler;
    }

    private void StateChangedHandler(StoveCounter.State state)
    {
        bool showVisual = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;

        _stoveOn.SetActive(showVisual);
        _particles.SetActive(showVisual);
    }
}
