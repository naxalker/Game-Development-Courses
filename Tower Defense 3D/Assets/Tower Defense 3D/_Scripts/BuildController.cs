using System;
using AYellowpaper.SerializedCollections;
using Zenject;

public class BuildController : IInitializable
{
    public event Action<BuildSlot> OnNewSlotSelected;
    public event Action<Tower> OnTowerBuilded;

    private BuildSlot _currentBuildSlot;
    private TowerFactory _towerFactory;

    private DiContainer _container;
    private SerializedDictionary<TowerType, TowerConfig> _towerConfigs;
    private LevelConfig _levelConfig;
    private CurrencyController _currencyController;

    public BuildController(
        DiContainer container,
        SerializedDictionary<TowerType, TowerConfig> towerConfigs,
        LevelConfig levelConfig,
        CurrencyController currencyController)
    {
        _container = container;
        _towerConfigs = towerConfigs;
        _levelConfig = levelConfig;
        _currencyController = currencyController;
    }

    public BuildSlot CurrentBuildSlot => _currentBuildSlot;

    public void Initialize()
    {
        _towerFactory = new TowerFactory(_container, _towerConfigs);
    }

    public void SetCurrentBuildSlot(BuildSlot buildSlot)
    {
        if (_currentBuildSlot != null && _currentBuildSlot == buildSlot) { return; }

        if (_currentBuildSlot != null)
        {
            _currentBuildSlot.DeselectBuildSlot();
        }

        _currentBuildSlot = buildSlot;

        OnNewSlotSelected?.Invoke(_currentBuildSlot);
    }

    public void BuildTower(TowerType towerType)
    {
        if (_currentBuildSlot == null) { return; }

        if (_towerConfigs[towerType].Cost <= _currencyController.Currency)
        {
            Tower tower = _towerFactory.CreateTower(towerType, _currentBuildSlot.transform.position);

            _currencyController.SpendCurrency(_towerConfigs[towerType].Cost);

            _currentBuildSlot.DeselectBuildSlot(true);
            _currentBuildSlot = null;

            OnTowerBuilded?.Invoke(tower);
        }
    }

    public int GetTowerCost(TowerType towerType) =>
        _towerConfigs.ContainsKey(towerType) ? _towerConfigs[towerType].Cost : 0;

    public bool IsTowerAvailableForLevel(TowerType towerType) => _levelConfig.TowerConfigs[towerType];
}
