using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ShakeEffect : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeMagnitude = 10f;
    [SerializeField] private int _vibrato = 20;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void DoShakeEffect()
    {
        _rectTransform.DOKill();
        _rectTransform.DOShakePosition(_shakeDuration, _shakeMagnitude, _vibrato);
        _rectTransform.DOScale(Vector3.one * 1.05f, _shakeDuration / 2)
            .SetLoops(2, LoopType.Yoyo)
            .From(Vector3.one);
    }
}
