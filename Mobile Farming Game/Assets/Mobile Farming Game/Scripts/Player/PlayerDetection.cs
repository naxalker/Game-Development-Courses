using System;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static Action<AppleTree> OnEnteredTreeZone;
    public static Action<AppleTree> OnExitedTreeZone;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChunkTrigger"))
        {
            Chunk chunk = other.GetComponentInParent<Chunk>();
            chunk.TryUnlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AppleTree tree))
        {
            TriggeredAppleTree(tree);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AppleTree tree))
        {
            ExitedAppleTreeZone(tree);
        }
    }

    private void TriggeredAppleTree(AppleTree tree)
    {
        OnEnteredTreeZone?.Invoke(tree);
    }

    private void ExitedAppleTreeZone(AppleTree tree)
    {
        OnExitedTreeZone?.Invoke(tree);
    }
}
