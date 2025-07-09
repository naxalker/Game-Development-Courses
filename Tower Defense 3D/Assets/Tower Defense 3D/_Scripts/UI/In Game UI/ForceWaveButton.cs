using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ForceWaveButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private Button _button;

    private WavesController _wavesController;

    [Inject]
    public void Construct(WavesController wavesController)
    {
        _wavesController = wavesController;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(() => _wavesController.ForceNextWave());

        _wavesController.OnFinalWaveStarted += FinalWaveStartedHandler;
    }

    private void OnDestroy()
    {
        _wavesController.OnFinalWaveStarted -= FinalWaveStartedHandler;
        DOTween.Kill(_canvasGroup);
    }

    private void FinalWaveStartedHandler()
    {
        _button.interactable = false;
        _canvasGroup.DOFade(0f, 1f)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
