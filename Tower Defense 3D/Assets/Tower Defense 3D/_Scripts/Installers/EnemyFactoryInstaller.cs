using UnityEngine;
using Zenject;

public class EnemyFactoryInstaller : MonoInstaller
{
    [SerializeField] private Enemy[] _enemiesPrefabs;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle().WithArguments(_enemiesPrefabs);
    }
}