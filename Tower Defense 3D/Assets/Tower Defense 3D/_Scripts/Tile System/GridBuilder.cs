using NaughtyAttributes;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _mainPrefab;
    [SerializeField] private Vector2 _gridSize = new Vector2(10, 10);

    [Button("Build Grid")]
    private void BuildGrid()
    {
        ClearGrid();

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int z = 0; z < _gridSize.y; z++)
            {
                CreateTile(x, z);
            }
        }
    }

    [Button("Clear Grid")]
    private void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void CreateTile(float xPos, float zPos)
    {
        Vector3 newPosition = new Vector3(xPos, 0, zPos);
        GameObject newTile = Instantiate(_mainPrefab, newPosition, Quaternion.identity, transform);
    }
}
