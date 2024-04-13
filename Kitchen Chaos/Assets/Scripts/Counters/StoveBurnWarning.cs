using UnityEngine;

public class StoveBurnWarning : MonoBehaviour
{
    private const float BurnShowProgressAmount = .5f;

    private StoveCounter _stoveCounter;

    private void Awake()
    {
        _stoveCounter = GetComponentInParent<StoveCounter>();
    }

    private void Start()
    {
        Hide();

        _stoveCounter.ProgressChanged += ProgressChangedHandler;
    }

    private void OnDestroy()
    {
        _stoveCounter.ProgressChanged -= ProgressChangedHandler;
    }

    private void ProgressChangedHandler(float progress)
    {
        bool show = _stoveCounter.IsFried && progress >= BurnShowProgressAmount;

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
