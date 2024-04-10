using System;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static Action<BaseCounter> AnyObjectPlaced;

    [SerializeField] protected Transform _counterTopPoint;

    protected KitchenObject _kitchenObject;

    public Transform KitchenObjectFollowTransform => _counterTopPoint;

    public KitchenObject KitchenObject
    {
        get { return _kitchenObject; }
        set { 
            _kitchenObject = value; 

            if (_kitchenObject != null)
            {
                AnyObjectPlaced?.Invoke(this);
            }
        }
    }

    public bool HasKitchenObject => _kitchenObject != null;

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public virtual void InteractAlternate(Player player) { }

    public abstract void Interact(Player player);
    
}
