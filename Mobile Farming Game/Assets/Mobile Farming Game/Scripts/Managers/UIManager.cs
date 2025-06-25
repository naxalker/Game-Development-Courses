using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _treeModePanel;
    [SerializeField] private GameObject _treeButton;
    [SerializeField] private GameObject _toolButtonsContainer;

    private void Awake()
    {
        PlayerDetection.OnEnteredTreeZone += EnteredTreeZoneHandler;
        PlayerDetection.OnExitedTreeZone += ExitedTreeZoneHandler;

        AppleTreeManager.OnTreeModeStarted += TreeModeStartedHandler;
        AppleTreeManager.OnTreeModeEnded += TreeModeEndedHandler;
    }

    private void Start()
    {
        SetGameMode();
    }

    private void OnDestroy()
    {
        PlayerDetection.OnEnteredTreeZone -= EnteredTreeZoneHandler;
        PlayerDetection.OnExitedTreeZone -= ExitedTreeZoneHandler;

        AppleTreeManager.OnTreeModeStarted -= TreeModeStartedHandler;
        AppleTreeManager.OnTreeModeEnded -= TreeModeEndedHandler;
    }

    private void EnteredTreeZoneHandler(AppleTree tree)
    {
        _treeButton.SetActive(true);
        _toolButtonsContainer.SetActive(false);
    }

    private void ExitedTreeZoneHandler(AppleTree tree)
    {
        _treeButton.SetActive(false);
        _toolButtonsContainer.SetActive(true);
    }

    private void TreeModeStartedHandler(AppleTree tree)
    {
        SetTreeMode();
    }

    private void TreeModeEndedHandler()
    {
        SetGameMode();
    }

    private void SetGameMode()
    {
        _gamePanel.SetActive(true);
        _treeModePanel.SetActive(false);
    }

    private void SetTreeMode()
    {
        _gamePanel.SetActive(false);
        _treeModePanel.SetActive(true);
    }
}
