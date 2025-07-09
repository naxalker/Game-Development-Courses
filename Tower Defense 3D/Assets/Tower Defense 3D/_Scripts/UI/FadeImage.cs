using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImage : MonoBehaviour
{
    private const float FADE_DURATION = 3f;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _image.DOFade(0f, FADE_DURATION)
            .From(1f)
            .SetEase(Ease.InQuad);
    }
}
