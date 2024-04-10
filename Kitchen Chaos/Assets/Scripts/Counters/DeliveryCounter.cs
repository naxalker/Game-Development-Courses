using Zenject;

public class DeliveryCounter : BaseCounter
{
    private DeliveryManager _deliveryManager;

    [Inject]
    private void Construct(DeliveryManager deliveryManager)
    {
        _deliveryManager = deliveryManager;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject)
        {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                _deliveryManager.DeliverRecipe(plateKitchenObject);

                player.KitchenObject.DestroySelf();
            }
        }
    }
}
