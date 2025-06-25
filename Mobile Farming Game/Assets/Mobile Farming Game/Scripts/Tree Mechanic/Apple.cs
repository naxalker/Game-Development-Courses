using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Apple : MonoBehaviour
{
    private enum State
    {
        Ready,
        Growing
    }

    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _shakeMultiplier;

    private Rigidbody _rigidbody;
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    private State _state;

    public bool IsFree => !_rigidbody.isKinematic;
    public bool IsReady => _state == State.Ready;

    private void Awake()
    {
        _state = State.Ready;

        _rigidbody = GetComponent<Rigidbody>();

        _initialPos = transform.position;
        _initialRot = transform.rotation;
    }

    public void Shake(float magnitude)
    {
        float realShakeMagnitude = magnitude * _shakeMultiplier;

        _renderer.material.SetFloat("_Magnitude", realShakeMagnitude);
    }

    public void Release()
    {
        _rigidbody.isKinematic = false;

        _state = State.Growing;

        _renderer.material.SetFloat("_Magnitude", 0);
    }

    public void Reset()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1f).setDelay(2f).setOnComplete(ForceReset);
    }

    private void ForceReset()
    {
        transform.position = _initialPos;
        transform.rotation = _initialRot;

        _rigidbody.isKinematic = true;

        LeanTween.scale(gameObject, Vector3.one, Random.Range(5f, 10f)).setOnComplete(SetReady);
    }

    private void SetReady()
    {
        _state = State.Ready;
    }
}
