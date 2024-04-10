using System;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event Action<CuttingCounter> AnyCut;

    public event Action<float> ProgressChanged;
    public event Action Cut;

    [SerializeField] private CuttingRecipeSO[] _recipes;

    private int _cuttingProgress;

    public override void Interact(Player player)
    {
        if (HasKitchenObject == false)
        {
            if (player.HasKitchenObject && HasSuitableRecipe(player.KitchenObject))
            {
                player.KitchenObject.Parent = this;

                _cuttingProgress = 0;
                ProgressChanged?.Invoke(0);
            }
        }
        else
        {
            if (player.HasKitchenObject)
            {
                if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(KitchenObject.KitchenObjectSO))
                    {
                        KitchenObject.DestroySelf();
                    }
                }
            }
            else
            {
                KitchenObject.Parent = player;
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject && player.HasKitchenObject == false)
        {
            var suitableRecipe = _recipes.FirstOrDefault(o => o.Input == KitchenObject.KitchenObjectSO);

            if (suitableRecipe != null)
            {
                _cuttingProgress++;

                Cut?.Invoke();
                AnyCut?.Invoke(this);

                ProgressChanged?.Invoke((float)_cuttingProgress / suitableRecipe.CuttingProgresMax);

                if (_cuttingProgress >= suitableRecipe.CuttingProgresMax)
                {
                    KitchenObject.DestroySelf();
                    KitchenObject.SpawnKitchenObject(suitableRecipe.Output, this);
                }
            }
        }
    }

    private bool HasSuitableRecipe(KitchenObject kitchenObject)
        => _recipes.FirstOrDefault(o => o.Input == kitchenObject.KitchenObjectSO) != null;
}
