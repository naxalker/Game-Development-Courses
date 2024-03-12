using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    public const string PlayerNameKey = "PlayerName";

    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private Button _connectButton;
    [SerializeField] private int _minLengthName = 1;
    [SerializeField] private int _maxLengthName = 12;

    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        _nameField.text = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        _connectButton.interactable = 
            _nameField.text.Length >= _minLengthName &&
            _nameField.text.Length <= _maxLengthName;
    }

    public void Connect()
    {
        PlayerPrefs.SetString(PlayerNameKey, _nameField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
