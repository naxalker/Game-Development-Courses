using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class TowerFactory
{
    private DiContainer _container;
    private SerializedDictionary<TowerType, TowerConfig> _towerConfigs;

    public TowerFactory(DiContainer container, SerializedDictionary<TowerType, TowerConfig> towerConfigs)
    {
        _container = container;
        _towerConfigs = towerConfigs;
    }

    public Tower CreateTower(TowerType towerType, Vector3 position)
    {
        TowerConfig towerConfig = _towerConfigs[towerType];

        Tower tower =
            _container.InstantiatePrefabForComponent<Tower>(
                towerConfig.TowerPrefab,
                position,
                Quaternion.identity,
                null
            );

        return tower;
    }
}
