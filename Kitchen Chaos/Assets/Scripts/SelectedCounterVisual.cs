using UnityEngine;
using Zenject;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject _selectedVisual;

    private Player _player;
    private ClearCounter _clearCounter;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Awake()
    {
        _clearCounter = GetComponentInParent<ClearCounter>();
    }

    private void OnEnable()
    {
        _player.SelectedCounterChanged += SelectedCounterChangedHandler;
    }

    private void OnDisable()
    {
        _player.SelectedCounterChanged -= SelectedCounterChangedHandler;
    }

    private void SelectedCounterChangedHandler(ClearCounter counter)
    {
        _selectedVisual.SetActive(counter == _clearCounter);
    }
}
