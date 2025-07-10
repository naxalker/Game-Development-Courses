using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool _canControl;
    [SerializeField] private Vector3 _levelCenterPoint;
    [SerializeField] private float _maxDistanceFromCenter;

    [Header("Movement Details")]
    [SerializeField] private float _movementSpeed = 120f;
    [SerializeField] private float _mouseMovementSpeed = 5f;
    [SerializeField] private float _edgeThreshold = 10f;
    [SerializeField] private float _edgeMovementSpeed = 10f;

    [Header("Rotation Details")]
    [SerializeField] private Transform _focusPoint;
    [SerializeField] private float _maxFocusDistance = 15f;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _maxPitch = 85f;
    [SerializeField] private float _minPitch = 5f;

    [Header("Zoom Details")]
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _minZoom = 3f;
    [SerializeField] private float _maxZoom = 15f;

    private Vector3 _lastMousePosition;

    private float _pitch;
    private Vector3 _targetZoomPosition;

    public void SetCanControl(bool canControl)
    {
        _canControl = canControl;
    }

    public void AdjustPitch(float pitch)
    {
        _pitch = Mathf.Clamp(pitch, _minPitch, _maxPitch);
    }

    public void AdjustKeyboardSensitivity(float sensitivity)
    {
        _movementSpeed = sensitivity;
    }

    public void AdjustMouseSensitivity(float sensitivity)
    {
        _mouseMovementSpeed = sensitivity;
    }

    private void Update()
    {
        if (!_canControl) { return; }

        HandleRotation();
        HandleMovement();
        HandleMouseMovement();
        // HandleEdgeMovement();
        HandleZoom();

        _focusPoint.position = transform.position + transform.forward * GetFocusPointDistance();
    }

    private void HandleMovement()
    {
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        if (vInput == 0 && hInput == 0) { return; }

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 movement = (flatForward * vInput + transform.right * hInput).normalized;
        movement *= _movementSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + movement;

        if (Vector3.Distance(_levelCenterPoint, newPosition) > _maxDistanceFromCenter)
        {
            newPosition = _levelCenterPoint + (newPosition - _levelCenterPoint).normalized * _maxDistanceFromCenter;
        }

        transform.position = newPosition;
    }

    private void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 positionDifference = Input.mousePosition - _lastMousePosition;
            Vector3 moveRight = transform.right * (-positionDifference.x) * _mouseMovementSpeed * Time.deltaTime;
            Vector3 moveForward = transform.forward * (-positionDifference.y) * _mouseMovementSpeed * Time.deltaTime;

            moveRight.y = 0;
            moveForward.y = 0;

            Vector3 movement = moveRight + moveForward;
            Vector3 newPosition = transform.position + movement;

            if (Vector3.Distance(_levelCenterPoint, newPosition) > _maxDistanceFromCenter)
            {
                newPosition = _levelCenterPoint + (newPosition - _levelCenterPoint).normalized * _maxDistanceFromCenter;
            }

            transform.position = newPosition;
            _lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleEdgeMovement()
    {
        Vector3 newPosition = transform.position;
        Vector3 mousePosition = Input.mousePosition;

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        if (mousePosition.x < _edgeThreshold)
        {
            newPosition -= transform.right * _edgeMovementSpeed * Time.deltaTime;
        }
        else if (mousePosition.x > Screen.width - _edgeThreshold)
        {
            newPosition += transform.right * _edgeMovementSpeed * Time.deltaTime;
        }

        if (mousePosition.y < _edgeThreshold)
        {
            newPosition -= flatForward * _edgeMovementSpeed * Time.deltaTime;
        }
        else if (mousePosition.y > Screen.height - _edgeThreshold)
        {
            newPosition += flatForward * _edgeMovementSpeed * Time.deltaTime;
        }

        if (Vector3.Distance(_levelCenterPoint, newPosition) > _maxDistanceFromCenter)
        {
            newPosition = _levelCenterPoint + (newPosition - _levelCenterPoint).normalized * _maxDistanceFromCenter;
        }

        transform.position = newPosition;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            float verticalRotation = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;

            transform.RotateAround(_focusPoint.position, Vector3.up, horizontalRotation);

            _pitch = Mathf.Clamp(_pitch - verticalRotation, _minPitch, _maxPitch);

            transform.RotateAround(_focusPoint.position, transform.right, _pitch - transform.eulerAngles.x);

            transform.LookAt(_focusPoint);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            Vector3 zoomDirection = transform.forward * scroll * _zoomSpeed;
            _targetZoomPosition = transform.position + zoomDirection;

            if (_targetZoomPosition.y < _minZoom && scroll > 0)
            {
                _targetZoomPosition = transform.position;
            }
            else if (_targetZoomPosition.y > _maxZoom && scroll < 0)
            {
                _targetZoomPosition = transform.position;
            }
        }
        else
        {
            _targetZoomPosition = transform.position;
        }

        transform.position = Vector3.Lerp(transform.position, _targetZoomPosition, 15f * Time.deltaTime);
    }

    private float GetFocusPointDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxFocusDistance))
        {
            return hit.distance;
        }

        return _maxFocusDistance;
    }
}
