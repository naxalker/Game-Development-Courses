using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;

    private WavesController _wavesController;
    private Coroutine _updateTextCoroutine;

    [Inject]
    public void Construct(WavesController wavesController)
    {
        _wavesController = wavesController;
    }

    private void Awake()
    {
        _wavesController.OnFinalWaveStarted += FinalWaveStartedHandler;
    }

    private void OnDestroy()
    {
        _wavesController.OnFinalWaveStarted -= FinalWaveStartedHandler;
    }

    private void Start()
    {
        _updateTextCoroutine = StartCoroutine(UpdateTextCoroutine());
    }

    private IEnumerator UpdateTextCoroutine()
    {
        while (true)
        {
            _waveText.text = $"Next Wave {Mathf.RoundToInt(_wavesController.WaveTimer)}";
            yield return new WaitForSeconds(1f);
        }
    }

    private void FinalWaveStartedHandler()
    {
        _waveText.text = "Final Wave!";
        StopCoroutine(_updateTextCoroutine);
    }
}
