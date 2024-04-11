using Zenject;

public class AsyncProcessorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }

    public override void Start()
    {
        base.Start();
        Loader.SetProcessor(Container.Resolve<AsyncProcessor>());
    }
}
