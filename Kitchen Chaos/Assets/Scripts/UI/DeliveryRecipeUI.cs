using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryRecipeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _recipeName;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Image _iconTemplate;

    private RecipeSO _recipeSO;

    public RecipeSO RecipeSO => _recipeSO;

    public void SetRecipe(RecipeSO recipeSO)
    {
        _recipeSO = recipeSO;

        _recipeName.text = recipeSO.RecipeName;

        foreach (Transform child in _iconContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(var ingredient in recipeSO.KitchenObjetSOList)
        {
            var icon = Instantiate(_iconTemplate, _iconContainer);
            icon.sprite = ingredient.Sprite;
        }
    }
}
