using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildController : IInitializable, ITickable, IDisposable
{
    public event Action<BuildSlot> OnNewSlotSelected;
    public event Action OnSlotDeselected;
    public event Action<Tower> OnTowerBuilded;

    private BuildSlot _currentBuildSlot;
    private TowerFactory _towerFactory;
    private TowerPreview _towerPreview;

    private readonly DiContainer _container;
    private readonly SerializedDictionary<TowerType, TowerConfig> _towerConfigs;
    private readonly LevelConfig _levelConfig;
    private readonly BuildButtonsHolder _buildButtonsHolder;
    private readonly Material _transparentMaterial;
    private readonly Material _attackRangeMaterial;
    private readonly CurrencyController _currencyController;

    public BuildController(
        DiContainer container,
        SerializedDictionary<TowerType, TowerConfig> towerConfigs,
        LevelConfig levelConfig,
        BuildButtonsHolder buildButtonsHolder,
        Material transparentMaterial,
        Material attackRangeMaterial,
        CurrencyController currencyController)
    {
        _container = container;
        _towerConfigs = towerConfigs;
        _levelConfig = levelConfig;
        _buildButtonsHolder = buildButtonsHolder;
        _transparentMaterial = transparentMaterial;
        _attackRangeMaterial = attackRangeMaterial;
        _currencyController = currencyController;
    }

    public BuildSlot CurrentBuildSlot => _currentBuildSlot;

    public void Initialize()
    {
        _towerFactory = new TowerFactory(_container, _towerConfigs);

        _buildButtonsHolder.OnHide += ButtonsHolderHideHandler;
    }

    public void Dispose()
    {
        _buildButtonsHolder.OnHide -= ButtonsHolderHideHandler;
    }

    public void Tick()
    {
        if (Input.GetMouseButtonDown(0) && !MouseOverUILayerObject.IsPointerOverUI())
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f))
            {
                if (hit.collider.TryGetComponent(out BuildSlot buildSlot))
                {
                    if (_currentBuildSlot != buildSlot)
                    {
                        SetCurrentBuildSlot(buildSlot);
                    }
                }
                else if (_currentBuildSlot != null)
                {
                    DeselectBuildSlot();
                }
            }
            else if (_currentBuildSlot != null)
            {
                DeselectBuildSlot();
            }
        }
    }

    public void SetCurrentBuildSlot(BuildSlot buildSlot)
    {
        if (_currentBuildSlot != null && _currentBuildSlot == buildSlot) { return; }

        _currentBuildSlot?.HideBuildSlot();

        _currentBuildSlot = buildSlot;

        if (_towerPreview != null)
        {
            _towerPreview.transform.position = new Vector3(
                    _currentBuildSlot.transform.position.x,
                    .5f,
                    _currentBuildSlot.transform.position.z
            );

            _currentBuildSlot.HideBuildSlot(true);
        }

        OnNewSlotSelected?.Invoke(_currentBuildSlot);
    }

    public void BuildTower(TowerType towerType)
    {
        if (_currentBuildSlot == null) { return; }

        UnityEngine.Object.Destroy(_towerPreview.gameObject);

        if (_currencyController.TrySpendCurrency(_towerConfigs[towerType].Cost))
        {
            Tower tower = _towerFactory.CreateTower(
                towerType,
                new Vector3(_currentBuildSlot.transform.position.x, .5f, _currentBuildSlot.transform.position.z)
            );

            DeselectBuildSlot(true, true);

            OnTowerBuilded?.Invoke(tower);
        }
    }

    public void CreatePreviewTower(TowerType towerType)
    {
        if (_currentBuildSlot == null) { return; }

        _currentBuildSlot.HideBuildSlot(true, false);

        if (_towerPreview != null)
        {
            UnityEngine.Object.Destroy(_towerPreview.gameObject);
        }

        Tower tower = _towerFactory.CreateTower(
            towerType,
            new Vector3(_currentBuildSlot.transform.position.x, .5f, _currentBuildSlot.transform.position.z)
        );

        _towerPreview = tower.gameObject.AddComponent<TowerPreview>();
        _towerPreview.Setup(tower, _transparentMaterial, _attackRangeMaterial);
    }

    public void DestroyPreviewTower()
    {
        _currentBuildSlot?.ShowBuildSlot(true);

        if (_towerPreview != null)
        {
            UnityEngine.Object.Destroy(_towerPreview.gameObject);
            _towerPreview = null;
        }
    }

    public int GetTowerCost(TowerType towerType) =>
        _towerConfigs.ContainsKey(towerType) ? _towerConfigs[towerType].Cost : 0;

    public bool IsTowerAvailableForLevel(TowerType towerType) => _levelConfig.TowerConfigs[towerType];

    private void DeselectBuildSlot(bool instant = false, bool blocked = false)
    {
        if (_currentBuildSlot == null) { return; }

        _currentBuildSlot.HideBuildSlot(instant, blocked);
        _currentBuildSlot = null;
        OnSlotDeselected?.Invoke();
    }

    private void ButtonsHolderHideHandler()
    {
        DeselectBuildSlot();
    }
}
