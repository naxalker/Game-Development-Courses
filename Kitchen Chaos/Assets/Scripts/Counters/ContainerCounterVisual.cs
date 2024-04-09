using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OpenClose = "OpenClose";

    [SerializeField] private Animator _animator;
    
    private ContainerCounter _counter;

    private void Awake()
    {
        _counter = GetComponentInParent<ContainerCounter>();
    }

    private void OnEnable()
    {
        _counter.PlayerGrabbedObject += PlayerGrabbedObjectHandler;
    }

    private void OnDisable()
    {
        _counter.PlayerGrabbedObject -= PlayerGrabbedObjectHandler;
    }

    private void PlayerGrabbedObjectHandler()
    {
        _animator.SetTrigger(OpenClose);
    }
}
