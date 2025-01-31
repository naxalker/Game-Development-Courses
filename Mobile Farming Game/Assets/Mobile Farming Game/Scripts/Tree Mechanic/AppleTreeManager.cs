using UnityEngine;

public class AppleTreeManager : MonoBehaviour
{
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
        _lastTriggeredTree.EnableCam();
    }

    private void EnteredTreeZoneHandler(AppleTree tree)
    {
        _lastTriggeredTree = tree;
    }
}
