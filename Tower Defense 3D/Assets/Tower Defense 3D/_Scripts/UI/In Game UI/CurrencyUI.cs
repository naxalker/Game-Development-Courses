using System;
using TMPro;
using UnityEngine;
using Zenject;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;

    private CurrencyController _currencyController;
    private ShakeEffect _shakeEffect;

    [Inject]
    private void Construct(CurrencyController currencyController)
    {
        _currencyController = currencyController;
    }

    private void Awake()
    {
        _currencyController.OnCurrencyChanged += UpdateText;
        _currencyController.OnInsufficientCurrency += InsufficientCurrencyHandler;

        _shakeEffect = GetComponent<ShakeEffect>();
    }

    private void OnDestroy()
    {
        _currencyController.OnCurrencyChanged -= UpdateText;
        _currencyController.OnInsufficientCurrency -= InsufficientCurrencyHandler;
    }

    private void Start()
    {
        UpdateText(_currencyController.Currency);
    }

    private void UpdateText(int currency)
    {
        _currencyText.text = $"Resources {currency}";
    }

    private void InsufficientCurrencyHandler()
    {
        _shakeEffect.DoShakeEffect();
    }
}
