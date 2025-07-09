using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HoverButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float SCALE_FACTOR = 1.1f;
    private const float ANIMATION_DURATION = 0.2f;

    [SerializeField] private bool _scaledTime = true;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_scaledTime)
        {
            _button.transform.DOScale(SCALE_FACTOR, ANIMATION_DURATION)
            .From(Vector3.one);
        }
        else
        {
            _button.transform.DOScale(SCALE_FACTOR, ANIMATION_DURATION)
                .From(Vector3.one)
                .SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_scaledTime)
        {
            _button.transform.DOScale(1f, ANIMATION_DURATION)
                .From(Vector3.one * SCALE_FACTOR);
        }
        else
        {
            _button.transform.DOScale(1f, ANIMATION_DURATION)
                .From(Vector3.one * SCALE_FACTOR)
                .SetUpdate(true);
        }
    }
}
