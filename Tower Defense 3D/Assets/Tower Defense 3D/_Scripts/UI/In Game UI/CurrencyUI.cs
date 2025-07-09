using TMPro;
using UnityEngine;
using Zenject;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;

    private CurrencyController _currencyController;

    [Inject]
    private void Construct(CurrencyController currencyController)
    {
        _currencyController = currencyController;
    }

    private void Awake()
    {
        _currencyController.OnCurrencyChanged += UpdateText;
    }

    private void OnDestroy()
    {
        _currencyController.OnCurrencyChanged -= UpdateText;
    }

    private void Start()
    {
        UpdateText(_currencyController.Currency);
    }

    private void UpdateText(int currency)
    {
        _currencyText.text = $"Resources {currency}";
    }
}
