using UnityEngine;
using Zenject;

public class EnemyFactory
{
    private Enemy[] _enemiesPrefabs;
    private DiContainer _diContainer;

    public EnemyFactory(Enemy[] enemiesPrefabs, DiContainer diContainer)
    {
        _enemiesPrefabs = enemiesPrefabs;
        _diContainer = diContainer;
    }

    public Enemy CreateEnemy(EnemyType type, Vector3 position)
    {
        int index = (int)type;

        if (index < 0 || index >= _enemiesPrefabs.Length + 1)
        {
            Debug.LogError("Invalid enemy type");
            return null;
        }

        Enemy enemy = _diContainer.InstantiatePrefabForComponent<Enemy>(_enemiesPrefabs[index - 1], position, Quaternion.identity, null);

        return enemy;
    }
}
