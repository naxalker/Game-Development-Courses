using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    [Header("Wave Manager")]
    [SerializeField] private int[] _wavesDuration;

    [Header("Health Controller")]
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private Castle _castle;

    [Header("Build Controller")]
    [SerializeField]
    [SerializedDictionary("Tower Type", "Tower Config")]
    private SerializedDictionary<TowerType, TowerConfig> _towerConfigs;
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private BuildButtonsHolder _buildButtonsHolder;
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private Material _attackRangeMaterial;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<WavesController>()
            .AsSingle()
            .WithArguments(_wavesDuration);

        Container.BindInterfacesAndSelfTo<HealthController>()
            .AsSingle()
            .WithArguments(_maxHP, _castle);

        Container.BindInterfacesAndSelfTo<CurrencyController>()
            .AsSingle();

        Container.BindInterfacesAndSelfTo<BuildController>()
            .AsSingle()
            .WithArguments(_towerConfigs, _levelConfig, _buildButtonsHolder, _transparentMaterial, _attackRangeMaterial);
    }

}