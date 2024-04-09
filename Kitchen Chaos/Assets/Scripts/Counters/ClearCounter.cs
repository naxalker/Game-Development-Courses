using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    public override void Interact(Player player)
    {
        if (HasKitchenObject == false)
        {
            if (player.HasKitchenObject)
            {
                player.KitchenObject.Parent = this;
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
                else
                {
                    if (KitchenObject.TryGetPlate(out plate))
                    {
                        if (plate.TryAddIngredient(player.KitchenObject.KitchenObjectSO))
                        {
                            player.KitchenObject.DestroySelf();
                        }
                    }
                }
            }
            else
            {
                KitchenObject.Parent = player;
            }
        }
    }
}
