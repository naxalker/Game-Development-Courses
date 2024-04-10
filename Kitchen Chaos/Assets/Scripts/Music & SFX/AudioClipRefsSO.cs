using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    [field: SerializeField] public AudioClip[] Chop { get; private set; }
    [field: SerializeField] public AudioClip[] DeliveryFail { get; private set; }
    [field: SerializeField] public AudioClip[] DeliverySuccess { get; private set; }
    [field: SerializeField] public AudioClip[] Footstep { get; private set; }
    [field: SerializeField] public AudioClip[] ObjectDrop { get; private set; }
    [field: SerializeField] public AudioClip[] ObjectPickup { get; private set; }
    [field: SerializeField] public AudioClip[] StoveSizzle { get; private set; }
    [field: SerializeField] public AudioClip[] Trash { get; private set; }
    [field: SerializeField] public AudioClip[] Warning { get; private set; }
}
