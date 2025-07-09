using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfig", menuName = "Tower Defense 3D/Tower Config")]
public class TowerConfig : ScriptableObject
{
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public Tower TowerPrefab { get; private set; }
}
