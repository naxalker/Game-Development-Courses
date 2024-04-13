using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class DeliveryManager : ITickable
{
    public event Action<RecipeSO> RecipeSpawned;
    public event Action<RecipeSO> RecipeCompleted;
    public event Action RecipeSuccessed;
    public event Action RecipeFailed;

    private const float SpawnRecipeTimerMax = 4f;
    private const int WaitingRecipesMax = 4;

    private readonly RecipeListSO _recipeListSO;
    private readonly GameManager _gameManager;

    private List<RecipeSO> _waitingRecipes = new List<RecipeSO>();
    private float _spawnRecipeTimer;
    private int _successfulRecipesAmount;

    public DeliveryManager(RecipeListSO recipeListSO, GameManager gameManager)
    {
        _recipeListSO = recipeListSO;
        _gameManager = gameManager;
    }

    public List<RecipeSO> WaitingRecipes => _waitingRecipes;
    public int SuccessfulRecipesAmount => _successfulRecipesAmount;

    public void Tick()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            if (_gameManager.IsGamePlaying && _waitingRecipes.Count < WaitingRecipesMax)
            {
                RecipeSO recipeSO = _recipeListSO.AllRecipes[Random.Range(0, _recipeListSO.AllRecipes.Count)];
                _waitingRecipes.Add(recipeSO);

                RecipeSpawned?.Invoke(recipeSO);

                _spawnRecipeTimer = SpawnRecipeTimerMax;
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipes.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipes[i];

            if (waitingRecipeSO.KitchenObjetSOList.Count == plateKitchenObject.KitchenObjectSOList.Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.KitchenObjetSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.KitchenObjectSOList)
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    _successfulRecipesAmount++;

                    _waitingRecipes.RemoveAt(i);

                    RecipeCompleted?.Invoke(waitingRecipeSO);
                    RecipeSuccessed?.Invoke();
                    return;
                }
            }
        }

        RecipeFailed?.Invoke();
    }
}
