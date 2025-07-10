using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CameraController))]
public class CameraEffects : MonoBehaviour
{
    [SerializeField] private Vector3 _inMenuPosition;
    [SerializeField] private Quaternion _inMenuRotation;

    [SerializeField] private Vector3 _inGamePosition;
    [SerializeField] private Quaternion _inGameRotation;

    [Header("Screen Shake Settings")]
    [Range(.1f, 3f)]
    [SerializeField] private float _shakeDuration;
    [Range(.01f, .5f)]
    [SerializeField] private float _shakeMagnitude;

    private CameraController _cameraController;
    private BuildController _buildController;

    [Inject]
    private void Construct(BuildController buildController)
    {
        _buildController = buildController;
    }

    private void Awake()
    {
        _buildController.OnTowerBuilded += TowerBuildedHandler;
    }

    private void OnDestroy()
    {
        _buildController.OnTowerBuilded -= TowerBuildedHandler;
    }

    private void Start()
    {
        _cameraController = GetComponent<CameraController>();
        SwitchToMenuView();
    }

    public void SwitchToMenuView()
    {
        StartCoroutine(ChangePositionAndRotation(_inMenuPosition, _inMenuRotation));
        _cameraController.AdjustPitch(_inMenuRotation.eulerAngles.x);
    }

    public void SwitchToGameView()
    {
        StartCoroutine(ChangePositionAndRotation(_inGamePosition, _inGameRotation));
        _cameraController.AdjustPitch(_inGameRotation.eulerAngles.x);
    }

    public void TriggerScreenShake(float shakeDuration, float shakeMagnitude)
    {
        StartCoroutine(ScreenShake(shakeDuration, shakeMagnitude));
    }

    private void TowerBuildedHandler(Tower tower)
    {
        TriggerScreenShake(_shakeDuration, _shakeMagnitude);
    }

    private IEnumerator ChangePositionAndRotation(
        Vector3 targetPosition,
        Quaternion targetRotation,
        float duration = 3f,
        float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        _cameraController.SetCanControl(false);

        float time = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);

            time += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        _cameraController.SetCanControl(true);
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _cameraController.transform.position = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _cameraController.transform.position = originalPosition;
    }
}
