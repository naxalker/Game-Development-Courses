using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] private Transform _visuals;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _verticalRotationSpeed;

    private void Update()
    {
        AlignWithSlope();
    }

    private void AlignWithSlope()
    {
        if (_visuals == null) { return; }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f, _groundLayer))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            _visuals.rotation = Quaternion.Slerp(_visuals.rotation, targetRotation, Time.deltaTime * _verticalRotationSpeed);
        }
    }

}
