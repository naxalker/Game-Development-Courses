using System;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] protected KitchenObjectSO _kitchenObjectSO;

    public event Action PlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject == false)
        {
            KitchenObject.SpawnKitchenObject(_kitchenObjectSO, player);

            PlayerGrabbedObject?.Invoke();
        }
    }
}
