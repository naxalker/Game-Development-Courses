using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private ClearCounter _clearCounter;

    public KitchenObjectSO KitchenObjectSO => _kitchenObjectSO;
    public ClearCounter ClearCounter
    {
        get { return _clearCounter; }
        set { 
            if (_clearCounter != null)
            {
                _clearCounter.ClearKitchenObject();
            }

            _clearCounter = value;

            if (_clearCounter.HasKitchenObject)
            {
                Debug.LogError("Counter already has a KitchenObject!");
            }

            _clearCounter.KitchenObject = this;

            transform.parent = _clearCounter.KitchenObjectFollowTransform;
            transform.localPosition = Vector3.zero;
        }
    }
}
