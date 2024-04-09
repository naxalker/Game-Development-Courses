using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSO;
        public GameObject GameObject;
    }

    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSoGameObjectList;

    private PlateKitchenObject _plate;

    private void Awake()
    {
        _plate = GetComponentInParent<PlateKitchenObject>();
    }

    private void Start()
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSoGameObjectList)
        {
            kitchenObjectSOGameObject.GameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _plate.IngredientAdded += IngredientAddedHandler;
    }

    private void OnDisable()
    {
        _plate.IngredientAdded -= IngredientAddedHandler;
    }

    private void IngredientAddedHandler(KitchenObjectSO kitchenObjectSO)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSoGameObjectList)
        {
            if (kitchenObjectSOGameObject.KitchenObjectSO == kitchenObjectSO) {
                kitchenObjectSOGameObject.GameObject.SetActive(true);
            }
        }
    }
}
