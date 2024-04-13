using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StoveBurnFlashingBar : MonoBehaviour
{
    private const string IsFlashing = "IsFlashing";
    private const float BurnShowProgressAmount = .5f;

    private StoveCounter _stoveCounter;
    private Animator _animator;

    private void Awake()
    {
        _stoveCounter = GetComponentInParent<StoveCounter>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetBool(IsFlashing, false);

        _stoveCounter.ProgressChanged += ProgressChangedHandler;
    }

    private void OnDestroy()
    {
        _stoveCounter.ProgressChanged -= ProgressChangedHandler;
    }

    private void ProgressChangedHandler(float progress)
    {
        bool show = _stoveCounter.IsFried && progress >= BurnShowProgressAmount;

        _animator.SetBool(IsFlashing, show);
    }
}
