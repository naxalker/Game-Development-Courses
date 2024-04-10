using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingle : MonoBehaviour
{
    [SerializeField] private Image _iconImage;

    public void SetKitchenObjectIcon(KitchenObjectSO kitchenObjectSO)
    {
        _iconImage.sprite = kitchenObjectSO.Sprite;
    }
}
