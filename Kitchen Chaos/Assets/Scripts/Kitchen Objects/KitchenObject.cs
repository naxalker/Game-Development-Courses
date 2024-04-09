using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _parent;

    public KitchenObjectSO KitchenObjectSO => _kitchenObjectSO;
    public IKitchenObjectParent Parent
    {
        get { return _parent; }
        set { 
            if (_parent != null)
            {
                _parent.ClearKitchenObject();
            }

            _parent = value;

            if (_parent.HasKitchenObject)
            {
                Debug.LogError("Counter already has a KitchenObject!");
            }

            _parent.KitchenObject = this;

            transform.parent = _parent.KitchenObjectFollowTransform;
            transform.localPosition = Vector3.zero;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        KitchenObject kitchenObject = Instantiate(kitchenObjectSO.Prefab).GetComponent<KitchenObject>();
        kitchenObject.Parent = parent;

        return kitchenObject;
    }

    public void DestroySelf()
    {
        Parent.ClearKitchenObject();
        Destroy(gameObject);
    }
}
