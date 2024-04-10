using UnityEngine;

public class PlateIcons : MonoBehaviour
{
    [SerializeField] private Transform _iconTemplate;

    private PlateKitchenObject _plateKitchenObject;

    private void Awake()
    {
        _plateKitchenObject = GetComponentInParent<PlateKitchenObject>();
    }

    private void OnEnable()
    {
        _plateKitchenObject.IngredientAdded += IngredientAddedHandler;
    }

    private void OnDisable()
    {
        _plateKitchenObject.IngredientAdded += IngredientAddedHandler;
    }

    private void IngredientAddedHandler(KitchenObjectSO kitchenObjectSO)
    {
        PlateIconSingle _icon = Instantiate(_iconTemplate, transform).GetComponent<PlateIconSingle>();
        _icon.SetKitchenObjectIcon(kitchenObjectSO);
    }
}
