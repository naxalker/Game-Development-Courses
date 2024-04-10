using System;

public class TrashCounter : BaseCounter
{
    public static Action<TrashCounter> AnyObjectTrashed;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject)
        {
            player.KitchenObject.DestroySelf();
            AnyObjectTrashed?.Invoke(this);
        }
    }
}
