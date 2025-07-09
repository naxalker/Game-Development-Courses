using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _attackVisuals;
    [SerializeField] private float _attackVisualsDuration;

    [Header("Emission Settings")]
    [SerializeField] private MeshRenderer _emissionRenderer;
    [SerializeField] private Color _startEmissionColor;
    [SerializeField] private Color _targetEmissionColor;
    [SerializeField] private float _targetIntensity;

    [Header("Rotor Visuals")]
    [SerializeField] private Transform _rotor;
    [SerializeField] private Transform _unloaded;
    [SerializeField] private Transform _loaded;

    [Header("Front String Settings")]
    [SerializeField] private LineRenderer _frontStringL;
    [SerializeField] private LineRenderer _frontStringR;

    [Space]

    [SerializeField] private Transform _frontStartL;
    [SerializeField] private Transform _frontStartR;
    [SerializeField] private Transform _frontEndL;
    [SerializeField] private Transform _frontEndR;

    [Header("Back String Settings")]

    [SerializeField] private LineRenderer _backStringL;
    [SerializeField] private LineRenderer _backStringR;

    [Space]

    [SerializeField] private Transform _backStartL;
    [SerializeField] private Transform _backStartR;
    [SerializeField] private Transform _backEndL;
    [SerializeField] private Transform _backEndR;

    private Material _glowMaterial;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _glowMaterial = new Material(_emissionRenderer.material);
        _emissionRenderer.material = _glowMaterial;
        _frontStringL.material = _glowMaterial;
        _frontStringR.material = _glowMaterial;
        _backStringL.material = _glowMaterial;
        _backStringR.material = _glowMaterial;
    }

    private void Update()
    {
        UpdateStringVisual(_frontStringL, _frontStartL, _frontEndL);
        UpdateStringVisual(_frontStringR, _frontStartR, _frontEndR);
        UpdateStringVisual(_backStringL, _backStartL, _backEndL);
        UpdateStringVisual(_backStringR, _backStartR, _backEndR);
    }

    public async void PlayAttackVFX(Transform startPoint, Transform endPoint)
    {
        _attackVisuals.enabled = true;
        float elapsed = 0f;

        while (elapsed < _attackVisualsDuration)
        {
            _attackVisuals.SetPosition(0, startPoint.position);

            if (endPoint != null)
            {
                _attackVisuals.SetPosition(1, endPoint.position);
            }

            await UniTask.Yield();

            elapsed += Time.deltaTime;
        }

        _attackVisuals.enabled = false;
    }

    public async void PlayReloadVFX(float duration)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        float startTime = Time.time;
        while (Time.time < startTime + duration && !cancellationToken.IsCancellationRequested)
        {
            float t = (Time.time - startTime) / duration;

            _glowMaterial.SetColor(
                "_EmissionColor",
                Color.Lerp(
                    _startEmissionColor,
                    _targetEmissionColor * _targetIntensity,
                    Mathf.Pow(t, 3)
                )
            );

            _rotor.transform.position = Vector3.Lerp(
                _unloaded.position,
                _loaded.position,
                1 - Mathf.Pow(1 - t, 3)
            );

            await UniTask.Yield();
        }
    }

    private void UpdateStringVisual(LineRenderer stringRenderer, Transform start, Transform end)
    {
        stringRenderer.SetPosition(0, start.position);
        stringRenderer.SetPosition(1, end.position);
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}
