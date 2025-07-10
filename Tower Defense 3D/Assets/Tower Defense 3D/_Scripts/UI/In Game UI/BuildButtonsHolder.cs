using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class BuildButtonsHolder : MonoBehaviour
{
    public event Action OnShow;
    public event Action OnHide;

    private const float SHOW_POSITION_Y = 150f;
    private const float HIDE_POSITION_Y = -150f;
    private const float ANIMATION_DURATION = .5f;

    private bool _isActive = false;
    private BuildButton[] _buildButtons;
    private BuildButton _selectedButton;

    private BuildController _buildController;
    private RectTransform _rectTransform;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    public bool IsActive => _isActive;
    public BuildButton SelectedButton => _selectedButton;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _buildController.OnNewSlotSelected += NewSlotSelectedHandler;
        _buildController.OnSlotDeselected += SlotDeselectedHandler;
    }

    private void OnDestroy()
    {
        _buildController.OnNewSlotSelected -= NewSlotSelectedHandler;
        _buildController.OnSlotDeselected -= SlotDeselectedHandler;
    }

    private void Start()
    {
        Hide(true);

        _buildButtons = GetComponentsInChildren<BuildButton>();
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_selectedButton != null)
                {
                    _selectedButton.TriggerButton();
                }
            }
            else
            {
                for (int i = 0; i < _buildButtons.Length; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    {
                        if (_selectedButton != null)
                        {
                            _selectedButton.DeselectButton();
                        }
                        _buildButtons[i].SelectButton();
                    }
                }
            }
        }
    }

    public void SetSelectedButton(BuildButton button)
    {
        _selectedButton = button;

        if (_selectedButton != null)
        {
            _buildController.CreatePreviewTower(_selectedButton.TowerType);
        }
        else
        {
            _buildController.DestroyPreviewTower();
        }
    }

    private void NewSlotSelectedHandler(BuildSlot slot)
    {
        Show();
    }

    private void SlotDeselectedHandler()
    {
        Hide();
    }

    private void Show(bool instant = false)
    {
        _rectTransform.DOKill();

        if (instant)
        {
            _rectTransform.anchoredPosition = new Vector2(0f, SHOW_POSITION_Y);
        }
        else
        {
            _rectTransform.DOAnchorPos(new Vector2(0f, SHOW_POSITION_Y), ANIMATION_DURATION);
        }

        _isActive = true;

        OnShow?.Invoke();
    }

    private void Hide(bool instant = false)
    {
        _rectTransform.DOKill();

        if (instant)
        {
            _rectTransform.anchoredPosition = new Vector2(0f, HIDE_POSITION_Y);
        }
        else
        {
            _rectTransform.DOAnchorPos(new Vector2(0f, HIDE_POSITION_Y), ANIMATION_DURATION);
        }

        _isActive = false;

        _selectedButton?.DeselectButton();

        _buildController.DestroyPreviewTower();

        OnHide?.Invoke();
    }
}
