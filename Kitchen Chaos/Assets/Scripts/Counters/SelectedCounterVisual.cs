using UnityEngine;
using Zenject;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject _selectedVisual;

    private Player _player;
    private BaseCounter _baseCounter;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        _baseCounter = GetComponentInParent<BaseCounter>();
    }

    private void OnEnable()
    {
        _player.SelectedCounterChanged += SelectedCounterChangedHandler;
    }

    private void OnDisable()
    {
        _player.SelectedCounterChanged -= SelectedCounterChangedHandler;
    }

    private void SelectedCounterChangedHandler(BaseCounter counter)
    {
        _selectedVisual.SetActive(counter == _baseCounter);
    }
}
