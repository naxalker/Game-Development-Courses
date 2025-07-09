using DG.Tweening;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 _axis;
    [SerializeField] private float _speed;

    private void Start()
    {
        Vector3 rotation = _axis * 360f;

        float duration = 360f / _speed;

        transform.DOLocalRotate(rotation, duration, RotateMode.FastBeyond360)
            .SetRelative()
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
