using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event Action<KitchenObjectSO> IngredientAdded;

    [SerializeField] private List<KitchenObjectSO> _validKitchenSOs;

    private List<KitchenObjectSO> _kitchenObjectSOList = new List<KitchenObjectSO>();

    public List<KitchenObjectSO> KitchenObjectSOList => _kitchenObjectSOList;

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (_validKitchenSOs.Contains(kitchenObjectSO) == false) return false;

        if (_kitchenObjectSOList.Contains(kitchenObjectSO)) return false;

        _kitchenObjectSOList.Add(kitchenObjectSO);
        IngredientAdded?.Invoke(kitchenObjectSO);
        return true;
    }
}
