using System;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    public static Action<CropType> OnAppleHarvested;

    [Header("Elements")]
    [SerializeField] private GameObject _treeCam;
    [SerializeField] private Renderer _renderer;
    private AppleTreeManager _appleTreeManager;

    [Header("Settings")]
    [SerializeField] private float _maxShakeMagnitude;
    [SerializeField] private float _shakeIncrement;
    [SerializeField] private Transform _appleParent;
    private float _shakeSliderValue;
    private float _shakeMagnitude;
    private bool _isShaking;

    public bool IsReady
    {
        get
        {
            for (int i = 0; i < _appleParent.childCount; i++)
            {
                if (_appleParent.GetChild(i).GetComponent<Apple>().IsReady == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void Initialize(AppleTreeManager appleTreeManager)
    {
        EnableCam();
        _shakeSliderValue = 0f;
        _appleTreeManager = appleTreeManager;
    }

    public void EnableCam()
    {
        _treeCam.SetActive(true);
    }

    public void DisableCam()
    {
        _treeCam.SetActive(false);
    }

    public void Shake()
    {
        _isShaking = true;

        TweenShake(_maxShakeMagnitude);

        UpdateShakeSlider();
    }

    public void StopShaking()
    {
        if (!_isShaking) { return; }

        _isShaking = false;

        TweenShake(0);
    }

    private void TweenShake(float targetMagnitude)
    {
        LeanTween.cancel(_renderer.gameObject);
        LeanTween.value(_renderer.gameObject, UpdateShakeMagnitude, _shakeMagnitude, targetMagnitude, 1f);
    }

    private void UpdateShakeMagnitude(float value)
    {
        _shakeMagnitude = value;
        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        foreach (Material material in _renderer.materials)
        {
            material.SetFloat("_Magnitude", _shakeMagnitude);
        }

        foreach (Transform appleT in _appleParent)
        {
            Apple apple = appleT.GetComponent<Apple>();

            if (!apple.IsFree)
            {
                apple.Shake(_shakeMagnitude);
            }
        }
    }

    private void UpdateShakeSlider()
    {
        _shakeSliderValue += _shakeIncrement;
        _appleTreeManager.UpdateShakeSlider(_shakeSliderValue);

        for (int i = 0; i < _appleParent.childCount; i++)
        {
            float applePercent = (float)i / _appleParent.childCount;

            Apple currentApple = _appleParent.GetChild(i).GetComponent<Apple>();

            if (_shakeSliderValue > applePercent && !currentApple.IsFree)
            {
                ReleaseApple(currentApple);
            }
        }

        if (_shakeSliderValue >= 1)
        {
            ExitTreeMode();
        }
    }

    private void ReleaseApple(Apple apple)
    {
        apple.Release();

        OnAppleHarvested?.Invoke(CropType.Apple);
    }

    private void ExitTreeMode()
    {
        _appleTreeManager.EndTreeMode();

        DisableCam();

        TweenShake(0);

        ResetApples();
    }

    private void ResetApples()
    {
        for (int i = 0; i < _appleParent.childCount; i++)
        {
            _appleParent.GetChild(i).GetComponent<Apple>().Reset();
        }
    }
}
