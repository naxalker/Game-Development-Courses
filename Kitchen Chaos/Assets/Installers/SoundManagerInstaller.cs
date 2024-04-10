using UnityEngine;
using Zenject;

public class SoundManagerInstaller : MonoInstaller
{
    [SerializeField] private AudioClipRefsSO _audioClipRefs;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SoundManager>().AsSingle().WithArguments(_audioClipRefs);
    }
}