using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject _treeButton;
    [SerializeField] private GameObject _toolButtonsContainer;

    private void Awake()
    {
        PlayerDetection.OnEnteredTreeZone += EnteredTreeZoneHandler;
        PlayerDetection.OnExitedTreeZone += ExitedTreeZoneHandler;
    }

    private void OnDestroy()
    {
        PlayerDetection.OnEnteredTreeZone -= EnteredTreeZoneHandler;
        PlayerDetection.OnExitedTreeZone -= ExitedTreeZoneHandler;
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
}
