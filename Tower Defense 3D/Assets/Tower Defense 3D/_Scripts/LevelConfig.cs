using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Tower Defense 3D/Level Config")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField]
    [SerializedDictionary("Tower Type", "Is Unlocked")]
    public SerializedDictionary<TowerType, bool> TowerConfigs { get; private set; } = new SerializedDictionary<TowerType, bool>()
    {
        { TowerType.Crossbow, false },
        { TowerType.Cannon, false },
        { TowerType.RapidFire, false },
        { TowerType.SpiderNest, false },
        { TowerType.Harpoon, false },
        { TowerType.Fan, false },
        { TowerType.Hammer, false }
    };
}
