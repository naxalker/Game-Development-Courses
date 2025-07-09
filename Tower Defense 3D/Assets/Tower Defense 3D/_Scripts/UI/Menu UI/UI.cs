using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] _uiElements;

    private SettingsPanel _settingsPanel;
    private MainMenu _mainMenu;

    private void Awake()
    {
        _settingsPanel = GetComponentInChildren<SettingsPanel>(true);
        _mainMenu = GetComponentInChildren<MainMenu>(true);

        SwitchTo(_settingsPanel.gameObject);
        SwitchTo(_mainMenu.gameObject);
    }

    private void OnEnable()
    {
        SwitchTo(_mainMenu.gameObject);
    }

    public void SwitchTo(GameObject uiToEnable)
    {
        foreach (GameObject uiElement in _uiElements)
        {
            uiElement.SetActive(false);
        }

        uiToEnable.SetActive(true);
    }

    public void Quit()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
