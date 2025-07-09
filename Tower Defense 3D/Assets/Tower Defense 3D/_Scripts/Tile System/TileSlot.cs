using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TileSlot : MonoBehaviour
{
    private MeshRenderer _meshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter _meshFilter => GetComponent<MeshFilter>();
    private Collider _collider => GetComponent<Collider>();
    private NavMeshSurface _navMesh => GetComponentInParent<NavMeshSurface>();
    private TileSetHolder tileSetHolder => GetComponentInParent<TileSetHolder>();

    public void SwitchTile(GameObject referenceTile)
    {
        gameObject.name = referenceTile.name;

        TileSlot newTile = referenceTile.GetComponent<TileSlot>();

        _meshFilter.mesh = newTile.GetMesh();
        _meshRenderer.material = newTile.GetMaterial();

        UpdateCollider(newTile.GetCollider());
        UpdateChildren(newTile);
        UpdateLayer(referenceTile);
        UpdateNavMesh();

        TurnIntoBuildSlotIfNeeded(referenceTile);
    }

    public Material GetMaterial() => _meshRenderer.sharedMaterial;

    public Mesh GetMesh() => _meshFilter.sharedMesh;

    public Collider GetCollider() => _collider;

    public void RotateTile(int dir)
    {
        transform.Rotate(0, 90 * dir, 0);
        UpdateNavMesh();
    }

    public void AdjustY(int verticalDir)
    {
        transform.position += new Vector3(0, .1f * verticalDir, 0);
        UpdateNavMesh();
    }

    public List<GameObject> GetAllChildren()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        return children;
    }

    public void UpdateCollider(Collider newCollider)
    {
        DestroyImmediate(_collider);

        if (newCollider is BoxCollider)
        {
            BoxCollider original = newCollider.GetComponent<BoxCollider>();
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();

            collider.center = original.center;
            collider.size = original.size;
        }
        else if (newCollider is MeshCollider)
        {
            MeshCollider original = newCollider.GetComponent<MeshCollider>();
            MeshCollider collider = gameObject.AddComponent<MeshCollider>();

            collider.sharedMesh = original.sharedMesh;
            collider.convex = original.convex;
        }
    }

    private void TurnIntoBuildSlotIfNeeded(GameObject referenceTile)
    {
        BuildSlot buildSlot = GetComponent<BuildSlot>();

        if (referenceTile != tileSetHolder.TileField)
        {
            DestroyImmediate(buildSlot);
        }
        else
        {
            if (buildSlot == null)
            {
                buildSlot = gameObject.AddComponent<BuildSlot>();
            }
        }
    }

    private void UpdateChildren(TileSlot newTile)
    {
        foreach (GameObject obj in GetAllChildren())
        {
            DestroyImmediate(obj);
        }

        foreach (GameObject obj in newTile.GetAllChildren())
        {
            Instantiate(obj, transform);
        }
    }

    private void UpdateLayer(GameObject referenceObj) => gameObject.layer = referenceObj.layer;

    private void UpdateNavMesh() => _navMesh.BuildNavMesh();
}
