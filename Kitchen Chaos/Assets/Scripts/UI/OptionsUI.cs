using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button _soundFXButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _soundFXText;
    [SerializeField] private TMP_Text _musicText;

    private SoundManager _soundManager;
    private MusicManager _musicManager;
    private GameManager _gameManager;

    [Inject]
    private void Construct(SoundManager soundManager, MusicManager musicManager, GameManager gameManager)
    {
        _soundManager = soundManager;
        _musicManager = musicManager;
        _gameManager = gameManager;
    }

    private void Awake()
    {
        _soundFXButton.onClick.AddListener(() =>
        {
            _soundManager.ChangeVolume();
            UpdateVisual();
        });

        _musicButton.onClick.AddListener(() =>
        {
            _musicManager.ChangeVolume();
            UpdateVisual();
        });

        _closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        _gameManager.GameUnpaused += GameUnpausedHandler;

        UpdateVisual();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameManager.GameUnpaused -= GameUnpausedHandler;
    }

    private void UpdateVisual()
    {
        _soundFXText.text = "Sound Effects: " + Mathf.Round(_soundManager.Volume * 10f);
        _musicText.text = "Music: " + Mathf.Round(_musicManager.Volume * 10f);
    }

    private void GameUnpausedHandler()
    {
        gameObject.SetActive(false);
    }
}
