using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event Action<KitchenObjectSO> IngredientAdded;

    [SerializeField] private List<KitchenObjectSO> _validKitchenSOs;

    private List<KitchenObjectSO> _kitchenObjectSOs = new List<KitchenObjectSO>();

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (_validKitchenSOs.Contains(kitchenObjectSO) == false) return false;

        if (_kitchenObjectSOs.Contains(kitchenObjectSO)) return false;

        _kitchenObjectSOs.Add(kitchenObjectSO);
        IngredientAdded?.Invoke(kitchenObjectSO);
        return true;
    }
}
