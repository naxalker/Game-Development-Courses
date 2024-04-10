using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private DeliveryRecipeUI _recipeTemplate;
    [SerializeField] private Transform _container;

    private DeliveryManager _deliveryManager;
    private List<DeliveryRecipeUI> _recipeUIList = new List<DeliveryRecipeUI>();

    [Inject]
    private void Construct(DeliveryManager deliveryManager)
    {
        _deliveryManager = deliveryManager;
    }

    private void OnEnable()
    {
        _deliveryManager.RecipeSpawned += RecipeSpawnedHandler;
        _deliveryManager.RecipeCompleted += RecipeCompletedHandler;
    }

    private void OnDisable()
    {
        _deliveryManager.RecipeSpawned -= RecipeSpawnedHandler;
        _deliveryManager.RecipeCompleted -= RecipeCompletedHandler;
    }

    private void RecipeSpawnedHandler(RecipeSO recipeSO)
    {
        DeliveryRecipeUI recipeUI = Instantiate(_recipeTemplate, _container);
        recipeUI.SetRecipe(recipeSO);
        _recipeUIList.Add(recipeUI);
    }

    private void RecipeCompletedHandler(RecipeSO recipeSO)
    {
        DeliveryRecipeUI recipeUI = _recipeUIList.FirstOrDefault(o => o.RecipeSO == recipeSO);
        Destroy(recipeUI.gameObject);
        _recipeUIList.Remove(recipeUI);
    }
}
