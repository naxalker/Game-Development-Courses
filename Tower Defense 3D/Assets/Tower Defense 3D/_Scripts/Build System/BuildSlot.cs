using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BuildController _buildController;

    private bool _isBlocked = false;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_buildController.CurrentBuildSlot == this || _isBlocked) { return; }

        MoveUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_buildController.CurrentBuildSlot == this || _isBlocked) { return; }

        MoveDown();
    }

    public void ShowBuildSlot(bool instant = false)
    {
        MoveUp(instant);
    }

    public void HideBuildSlot(bool instant = false, bool blocked = false)
    {
        _isBlocked = blocked;
        MoveDown(instant);
    }

    private void MoveUp(bool instant = false)
    {
        transform.DOKill();

        if (instant)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
        }
        else
        {
            transform.DOLocalMoveY(0.5f, 0.2f)
                    .From(transform.position)
                    .SetEase(Ease.OutQuad);
        }
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
