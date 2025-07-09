using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BuildButton : MonoBehaviour
{
    [SerializeField] private TowerType _towerType;
    [SerializeField] private TextMeshProUGUI _costText;

    private Button _button;
    private BuildController _buildController;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnButtonClicked);
    }

    private void Start()
    {
        if (_buildController.IsTowerAvailableForLevel(_towerType) == false)
        {
            gameObject.SetActive(false);
        }

        _costText.text = _buildController.GetTowerCost(_towerType).ToString();
    }

    private void OnButtonClicked()
    {
        if (_buildController.CurrentBuildSlot == null) { return; }

        _buildController.BuildTower(_towerType);
    }
}
