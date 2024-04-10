using UnityEngine;
using Zenject;

public class DeliveryManagerInstaller : MonoInstaller
{
    [SerializeField] private RecipeListSO _recipeListSO;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DeliveryManager>().AsSingle().WithArguments(_recipeListSO);
    }
}