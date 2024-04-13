using System;
using System.Linq;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event Action<State> StateChanged;
    public event Action<float> ProgressChanged;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] _fryingRecipes;
    [SerializeField] private BurningRecipeSO[] _burningRecipes;

    private State _state;

    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipe;

    private float _burningTimer;
    private BurningRecipeSO _burningRecipe;

    public bool IsFried => _state == State.Fried;

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                break;

            case State.Frying:
                _fryingTimer += Time.deltaTime;

                ProgressChanged?.Invoke(_fryingTimer / _fryingRecipe.FryingTimerMax);

                if (_fryingTimer > _fryingRecipe.FryingTimerMax)
                {
                    KitchenObject.DestroySelf();

                    KitchenObject.SpawnKitchenObject(_fryingRecipe.Output, this);

                    _state = State.Fried;
                    _burningRecipe = GetSuitableBurningRecipe(KitchenObject);
                    _burningTimer = 0f;

                    StateChanged?.Invoke(_state);
                }
                break;

            case State.Fried:
                _burningTimer += Time.deltaTime;

                ProgressChanged?.Invoke(_burningTimer / _burningRecipe.BurningTimerMax);

                if (_burningTimer > _burningRecipe.BurningTimerMax)
                {
                    KitchenObject.DestroySelf();

                    KitchenObject.SpawnKitchenObject(_burningRecipe.Output, this);

                    _state = State.Burned;

                    StateChanged?.Invoke(_state);
                    ProgressChanged?.Invoke(0);
                }
                break;

            case State.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject == false)
        {
            if (player.HasKitchenObject)
            {
                if (HasSuitableFryingRecipe(player.KitchenObject))
                {
                    player.KitchenObject.Parent = this;

                    _fryingRecipe = GetSuitableFryingRecipe(KitchenObject);

                    _state = State.Frying;
                    _fryingTimer = 0f;

                    StateChanged?.Invoke(_state);
                    ProgressChanged?.Invoke(0);
                }
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

                        _state = State.Idle;

                        StateChanged?.Invoke(_state);
                        ProgressChanged?.Invoke(0);
                    }
                }
            }
            else
            {
                KitchenObject.Parent = player;

                _state = State.Idle;

                StateChanged?.Invoke(_state);
                ProgressChanged?.Invoke(0);
            }
        }
    }

    private bool HasSuitableFryingRecipe(KitchenObject kitchenObject)
        => _fryingRecipes.FirstOrDefault(o => o.Input == kitchenObject.KitchenObjectSO) != null;
    
    private FryingRecipeSO GetSuitableFryingRecipe(KitchenObject kitchenObject)
        => _fryingRecipes.FirstOrDefault(o => o.Input == kitchenObject.KitchenObjectSO);

    private BurningRecipeSO GetSuitableBurningRecipe(KitchenObject kitchenObject)
        => _burningRecipes.FirstOrDefault(o => o.Input == kitchenObject.KitchenObjectSO);
}
