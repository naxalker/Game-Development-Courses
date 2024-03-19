using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded == false) { return; }

        Instantiate(_prefab, transform.position, Quaternion.identity);
    }
}
