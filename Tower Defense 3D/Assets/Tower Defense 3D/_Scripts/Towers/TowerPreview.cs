using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    public void Setup(Tower tower, Material transparentMaterial, Material attackRangeMaterial)
    {
        float attackRange = tower.AttackRange;

        DestroyExtraComponents();
        MakeMeshRenderersTransparent(transparentMaterial);
        CreateAttackRadiusDisplay(attackRange, attackRangeMaterial);
    }

    private void DestroyExtraComponents()
    {
        foreach (Component component in GetComponents<Component>())
        {
            if (!(component is Transform) && !(component is TowerPreview))
            {
                Destroy(component);
            }
        }
    }

    private void MakeMeshRenderersTransparent(Material material)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            if (meshRenderer.material != null)
            {
                meshRenderer.material = material;
            }
        }
    }

    private void CreateAttackRadiusDisplay(float attackRange, Material material)
    {
        TowerAttackRadiusDisplay towerAttackRadiusDisplay =
            gameObject.AddComponent<TowerAttackRadiusDisplay>();

        towerAttackRadiusDisplay.CreateCircle(attackRange, material);
    }
}
