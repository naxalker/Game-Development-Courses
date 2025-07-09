using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private BuildController _buildController;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_buildController.CurrentBuildSlot == this) { return; }

        MoveUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_buildController.CurrentBuildSlot == this) { return; }

        MoveDown();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        _buildController.SetCurrentBuildSlot(this);
    }

    public void DeselectBuildSlot(bool instant = false)
    {
        MoveDown(instant);
    }

    private void MoveUp()
    {
        transform.DOKill();

        transform.DOLocalMoveY(0.5f, 0.2f)
                    .From(transform.position)
                    .SetEase(Ease.OutQuad);
    }

    private void MoveDown(bool instant = false)
    {
        transform.DOKill();

        if (instant)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        }
        else
        {
            transform.DOLocalMoveY(0f, 0.2f)
            .From(transform.position)
            .SetEase(Ease.InQuad);
        }
    }
}
