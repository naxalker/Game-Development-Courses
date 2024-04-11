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
        _optionsButton.onClick.AddListener(() => _optionsUI.gameObject.SetActive(true));
    }

    private void Start()
    {
        _gameManager.GamePaused += GamePausedHandler;
        _gameManager.GameUnpaused += GameUnpausedHandler;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameManager.GamePaused -= GamePausedHandler;
        _gameManager.GameUnpaused -= GameUnpausedHandler;
    }

    private void GamePausedHandler()
    {
        gameObject.SetActive(true);
    }

    private void GameUnpausedHandler()
    {
        gameObject.SetActive(false);
    }
}
