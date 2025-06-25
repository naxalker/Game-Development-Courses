using System;
using UnityEngine;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{
    public static Action<AppleTree> OnTreeModeStarted;
    public static Action OnTreeModeEnded;

    [Header("References")]
    [SerializeField] private Slider _treeSlider;

    [Header("Settings")]
    private AppleTree _lastTriggeredTree;

    private void Awake()
    {
        PlayerDetection.OnEnteredTreeZone += EnteredTreeZoneHandler;
    }

    private void OnDestroy()
    {
        PlayerDetection.OnEnteredTreeZone -= EnteredTreeZoneHandler;
    }

    public void TreeButtonClickedHandler()
    {
        if (!_lastTriggeredTree.IsReady) { return; }

        StartTreeMode();
    }

    public void UpdateShakeSlider(float value)
    {
        _treeSlider.value = value;
    }

    public void EndTreeMode()
    {
        OnTreeModeEnded?.Invoke();
    }

    private void EnteredTreeZoneHandler(AppleTree tree)
    {
        _lastTriggeredTree = tree;
    }

    private void StartTreeMode()
    {
        _lastTriggeredTree.Initialize(this);

        OnTreeModeStarted?.Invoke(_lastTriggeredTree);

        UpdateShakeSlider(0);
    }
}
