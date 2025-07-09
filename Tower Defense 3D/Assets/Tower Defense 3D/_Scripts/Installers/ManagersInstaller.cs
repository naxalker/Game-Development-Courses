using UnityEngine;
using Zenject;

public class ManagersInstaller : MonoInstaller
{
    [Header("Wave Manager")]
    [SerializeField] private int[] _wavesDuration;

    [Header("Health Controller")]
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private Castle _castle;

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
    }

}