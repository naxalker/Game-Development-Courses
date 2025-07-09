using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class WaveTimerUI : MonoBehaviour
{
    private const float SHOW_POSITION_Y = -80f;
    private const float HIDE_POSITION_Y = 180f;

    [SerializeField] private TextMeshProUGUI _waveText;

    private WavesController _wavesController;
    private Coroutine _updateTextCoroutine;
    private RectTransform _rectTransform;

    [Inject]
    public void Construct(WavesController wavesController)
    {
        _wavesController = wavesController;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _wavesController.OnFinalWaveStarted += FinalWaveStartedHandler;
        _wavesController.OnWaveStarted += WaveStartedHandler;
        _wavesController.OnWaveCompleted += WaveCompletedHandler;
    }

    private void OnDestroy()
    {
        _wavesController.OnFinalWaveStarted -= FinalWaveStartedHandler;
        _wavesController.OnWaveStarted -= WaveStartedHandler;
        _wavesController.OnWaveCompleted -= WaveCompletedHandler;
    }

    private void Start()
    {
        // _updateTextCoroutine = StartCoroutine(UpdateTextCoroutine());
        Hide(true);
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

    private void WaveStartedHandler(int waveIndex)
    {
        Hide();
    }

    private void WaveCompletedHandler(int waveIndex)
    {
        Show();
    }

    private void Show(bool instant = false)
    {
        gameObject.SetActive(true);

        if (instant)
        {
            _rectTransform.anchoredPosition = new Vector2(0f, SHOW_POSITION_Y);
        }
        else
        {
            _rectTransform.DOAnchorPos(new Vector2(0f, SHOW_POSITION_Y), 1f)
                .From(new Vector2(0f, HIDE_POSITION_Y));
        }

        _updateTextCoroutine = StartCoroutine(UpdateTextCoroutine());
    }

    private void Hide(bool instant = false)
    {
        if (instant)
        {
            gameObject.SetActive(false);

            _rectTransform.anchoredPosition = new Vector2(0f, HIDE_POSITION_Y);
        }
        else
        {
            _rectTransform.DOAnchorPos(new Vector2(0f, HIDE_POSITION_Y), 1f)
                        .From(new Vector2(0f, SHOW_POSITION_Y))
                        .OnComplete(() => gameObject.SetActive(false));
        }

        if (_updateTextCoroutine != null)
            StopCoroutine(_updateTextCoroutine);
    }
}
