using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;

    private void Awake()
    {
        _hasProgress = GetComponentInParent<IHasProgress>();
    }

    private void Start()
    {
        _hasProgress.ProgressChanged += ProgressChangedHandler;

        _barImage.fillAmount = 0;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _hasProgress.ProgressChanged -= ProgressChangedHandler;
    }

    private void ProgressChangedHandler(float fillValue)
    {
        fillValue = Mathf.Clamp01(fillValue);

        _barImage.fillAmount = fillValue;

        gameObject.SetActive(fillValue != 0 && fillValue != 1f);
    }
}
