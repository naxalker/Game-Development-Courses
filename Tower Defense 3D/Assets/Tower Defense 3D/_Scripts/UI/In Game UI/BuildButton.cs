using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float SCALE_FACTOR = 1.1f;
    private const float ANIMATION_DURATION = 0.2f;

    [SerializeField] private TowerType _towerType;
    [SerializeField] private TextMeshProUGUI _costText;

    private Button _button;
    private BuildController _buildController;

    private BuildButtonsHolder _buildButtonsHolder;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    public TowerType TowerType => _towerType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeselectButton();
    }

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnButtonClicked);

        _buildButtonsHolder = GetComponentInParent<BuildButtonsHolder>();
    }

    private void Start()
    {
        if (_buildController.IsTowerAvailableForLevel(_towerType) == false)
        {
            gameObject.SetActive(false);
        }

        _costText.text = _buildController.GetTowerCost(_towerType).ToString();
    }

    public void SelectButton()
    {
        if (_buildButtonsHolder.SelectedButton == this) { return; }

        _buildButtonsHolder.SetSelectedButton(this);
        transform.DOScale(SCALE_FACTOR, ANIMATION_DURATION)
                        .From(Vector3.one);
    }

    public void DeselectButton()
    {
        transform.DOScale(1f, ANIMATION_DURATION)
                        .From(Vector3.one * SCALE_FACTOR);
    }

    public void TriggerButton()
    {
        if (_buildController.CurrentBuildSlot == null) { return; }

        _buildController.BuildTower(_towerType);
    }

    private void OnButtonClicked()
    {
        if (_buildController.CurrentBuildSlot == null) { return; }

        _buildController.BuildTower(_towerType);
    }
}
