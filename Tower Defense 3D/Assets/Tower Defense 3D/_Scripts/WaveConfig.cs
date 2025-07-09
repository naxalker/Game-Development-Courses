using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Tower Defense 3D/Wave Config")]
public class WaveConfig : ScriptableObject
{
    [field: SerializeField]
    [SerializedDictionary("Enemy Type", "Count")]
    public SerializedDictionary<EnemyType, int> EnemiesCount { get; private set; }

    [field: SerializeField] public bool RandomizeWave { get; private set; }
    [field: SerializeField] public float EnemySpawnInterval { get; private set; } = 1f;
}
