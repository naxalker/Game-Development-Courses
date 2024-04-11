using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private OptionsUI _optionsUI;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _mainMenuButton;

    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Awake()
    {
        _resumeButton.onClick.AddListener(() => _gameManager.TogglePause());
        _mainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MainMenuScene));
        _optionsButton.onClick.AddListener(() =>
        {
            Hide();
            _optionsUI.Show(Show);
        });
    }

    private void Start()
    {
        _gameManager.GamePaused += GamePausedHandler;
        _gameManager.GameUnpaused += GameUnpausedHandler;

        Hide();
    }

    private void OnDestroy()
    {
        _gameManager.GamePaused -= GamePausedHandler;
        _gameManager.GameUnpaused -= GameUnpausedHandler;
    }

    private void GamePausedHandler()
    {
        Show();
    }

    private void GameUnpausedHandler()
    {
        Hide();
    }

    private void Show()
    {
        _resumeButton.Select();
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
