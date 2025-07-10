using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TowerAttackRadiusDisplay : MonoBehaviour
{
    private const int SEGMENTS = 50;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = SEGMENTS;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    public void CreateCircle(float radius, Material material)
    {
        _lineRenderer.enabled = true;

        _lineRenderer.material = material;

        float angle = 0f;
        Vector3 center = new Vector3(0f, .2f, 0f);

        for (int i = 0; i < SEGMENTS; i++)
        {
            float x = center.x + Mathf.Sin(angle) * radius;
            float z = center.z + Mathf.Cos(angle) * radius;
            _lineRenderer.SetPosition(i, new Vector3(x, center.y, z));
            angle += 2f * Mathf.PI / SEGMENTS;
        }
    }
}
