using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerShakeTreeAbility : MonoBehaviour
{
    [SerializeField] private float _distanceToTree;
    private PlayerAnimator _playerAnimator;
    private AppleTree _currentTree;
    private bool _isActive;
    private bool _isShaking;
    private Vector2 _previousMousePosition;

    private void Awake()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
        AppleTreeManager.OnTreeModeStarted += TreeModeStartedHandler;
        AppleTreeManager.OnTreeModeEnded += TreeModeEndedHandler;
    }

    private void Update()
    {
        if (_isActive && !_isShaking)
        {
            ManageTreeShaking();
        }
    }

    private void OnDestroy()
    {
        AppleTreeManager.OnTreeModeStarted -= TreeModeStartedHandler;
        AppleTreeManager.OnTreeModeEnded -= TreeModeEndedHandler;
    }

    private void TreeModeStartedHandler(AppleTree tree)
    {
        _currentTree = tree;

        _isActive = true;

        MoveTowardsTree();
    }

    private void TreeModeEndedHandler()
    {
        _currentTree = null;

        _isActive = false;
        _isShaking = false;

        LeanTween.delayedCall(.1f, () => _playerAnimator.StopShakeTreeAnimation());
    }

    private void MoveTowardsTree()
    {
        Vector3 treePos = _currentTree.transform.position;
        Vector3 dir = transform.position - treePos;

        Vector3 flatDir = dir;
        flatDir.y = 0;

        Vector3 targetPos = treePos + flatDir.normalized * _distanceToTree;

        _playerAnimator.ManageAnimations(-flatDir);

        LeanTween.move(gameObject, targetPos, .5f);
    }

    private void ManageTreeShaking()
    {
        if (!Input.GetMouseButton(0))
        {
            _currentTree.StopShaking();
            return;
        }

        float shakeMagnitude = Vector2.Distance(Input.mousePosition, _previousMousePosition);

        if (ShouldShake(shakeMagnitude))
        {
            Shake();
        }
        else
        {
            _currentTree.StopShaking();
        }

        _previousMousePosition = Input.mousePosition;
    }

    private bool ShouldShake(float shakeMagnitude)
    {
        float screenPercent = shakeMagnitude / Screen.width;
        return screenPercent >= .05f;
    }

    private void Shake()
    {
        _isShaking = true;

        _playerAnimator.PlayShakeTreeAnimation();
        _currentTree.Shake();

        LeanTween.delayedCall(.1f, () => _isShaking = false);
    }
}
