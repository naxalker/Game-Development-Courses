using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BlinkTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float BLINK_INTERVAL = 1f;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.DOKill();
        _text.DOFade(0f, BLINK_INTERVAL)
            .SetLoops(-1, LoopType.Yoyo)
            .From(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.DOKill();
        _text.DOFade(1f, BLINK_INTERVAL);
    }
}
