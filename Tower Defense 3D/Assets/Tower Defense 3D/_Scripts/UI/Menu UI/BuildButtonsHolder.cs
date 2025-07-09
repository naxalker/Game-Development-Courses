using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class BuildButtonsHolder : MonoBehaviour
{
    private const float SHOW_POSITION_Y = 150f;
    private const float HIDE_POSITION_Y = -150f;
    private const float ANIMATION_DURATION = .5f;

    private bool _isActive = false;

    private BuildController _buildController;
    private RectTransform _rectTransform;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _buildController.OnNewSlotSelected += NewSlotSelectedHandler;
        _buildController.OnTowerBuilded += SlotDeselectedHandler;
    }

    private void OnDestroy()
    {
        _buildController.OnNewSlotSelected -= NewSlotSelectedHandler;
        _buildController.OnTowerBuilded -= SlotDeselectedHandler;
    }

    private void Start()
    {
        Hide(true);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isActive)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
#endif

    private void NewSlotSelectedHandler(BuildSlot slot)
    {
        Show();
    }

    private void SlotDeselectedHandler(Tower tower)
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
    }
}
