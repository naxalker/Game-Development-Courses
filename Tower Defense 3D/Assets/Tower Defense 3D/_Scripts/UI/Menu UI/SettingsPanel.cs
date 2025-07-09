using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    [Header("Keyboard Sensetivity")]
    [SerializeField] private Slider _keyboardSensetivitySlider;
    [SerializeField] private TextMeshProUGUI _keyboardSensitivityText;
    [SerializeField] private float _minKeyboardSensitivity = 60f;
    [SerializeField] private float _maxKeyboardSensitivity = 240f;

    [Header("Mouse Sensetivity")]
    [SerializeField] private Slider _mouseSensetivitySlider;
    [SerializeField] private TextMeshProUGUI _mouseSensitivityText;
    [SerializeField] private float _minMouseSensitivity = 1f;
    [SerializeField] private float _maxMouseSensitivity = 10f;

    private void OnEnable()
    {
        _keyboardSensetivitySlider.value = PlayerPrefs.GetFloat("KeyboardSensitivity", 0.5f);
        _mouseSensetivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);

        ChangeKeyboardSensitivity(_keyboardSensetivitySlider.value);
        ChangeMouseSensitivity(_mouseSensetivitySlider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("KeyboardSensitivity", _keyboardSensetivitySlider.value);
        PlayerPrefs.SetFloat("MouseSensitivity", _mouseSensetivitySlider.value);
    }

    public void ChangeKeyboardSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(_minKeyboardSensitivity, _maxKeyboardSensitivity, value);
        _cameraController.AdjustKeyboardSensitivity(newSensitivity);
        _keyboardSensitivityText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void ChangeMouseSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(_minMouseSensitivity, _maxMouseSensitivity, value);
        _cameraController.AdjustMouseSensitivity(newSensitivity);
        _mouseSensitivityText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
