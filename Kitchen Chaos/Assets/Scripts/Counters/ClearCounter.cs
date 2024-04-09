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
            if (player.HasKitchenObject == false)
            {
                KitchenObject.Parent = player;
            }
        }
    }
}
