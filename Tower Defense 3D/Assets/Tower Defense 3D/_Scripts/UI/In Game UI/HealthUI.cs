using System;
using TMPro;
using UnityEngine;
using Zenject;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;

    private HealthController _healthController;
    private ShakeEffect _shakeEffect;

    [Inject]
    public void Construct(HealthController healthController)
    {
        _healthController = healthController;
    }

    private void Awake()
    {
        _shakeEffect = GetComponent<ShakeEffect>();

        _healthController.OnHealthChanged += HealthChangedHandler;
    }

    private void OnDestroy()
    {
        _healthController.OnHealthChanged -= HealthChangedHandler;
    }

    private void Start()
    {
        UpdateText(_healthController.CurrentHP, _healthController.MaxHP);
    }

    private void HealthChangedHandler(int currentHP)
    {
        UpdateText(currentHP, _healthController.MaxHP);
        _shakeEffect.DoShakeEffect();
    }

    private void UpdateText(int currentHP, int maxHP)
    {
        _healthText.text = $"Threat {maxHP - currentHP}/{maxHP}";
    }
}
