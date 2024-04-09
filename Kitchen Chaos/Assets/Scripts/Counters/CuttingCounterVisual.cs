using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string Cut = "Cut";

    [SerializeField] private Animator _animator;
    
    private CuttingCounter _counter;

    private void Awake()
    {
        _counter = GetComponentInParent<CuttingCounter>();
    }

    private void OnEnable()
    {
        _counter.Cut += CutHandler;
    }

    private void OnDisable()
    {
        _counter.Cut -= CutHandler;
    }

    private void CutHandler()
    {
        _animator.SetTrigger(Cut);
    }
}
