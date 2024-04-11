using UnityEngine;
using Zenject;

public class SoundAndMusicInstaller : MonoInstaller
{
    [SerializeField] private AudioClipRefsSO _audioClipRefs;
    [SerializeField] private AudioSource backgroundMusic;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SoundManager>().AsSingle().WithArguments(_audioClipRefs);
        Container.Bind<MusicManager>().AsSingle().WithArguments(backgroundMusic);
    }
}