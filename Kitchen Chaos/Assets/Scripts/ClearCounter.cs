using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private Transform _counterTopPoint;

    private KitchenObject _kitchenObject;

    public Transform KitchenObjectFollowTransform => _counterTopPoint;
    public KitchenObject KitchenObject
    {
        get { return _kitchenObject; }
        set { _kitchenObject = value; }
    }

    public bool HasKitchenObject => _kitchenObject != null;

    public void Interact()
    {
        if (_kitchenObject == null)
        {
            _kitchenObject = Instantiate(_kitchenObjectSO.Prefab, _counterTopPoint).GetComponent<KitchenObject>();
            _kitchenObject.ClearCounter = this;
        }
        else
        {
            Debug.Log(_kitchenObject.ClearCounter);
        }
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }
}
