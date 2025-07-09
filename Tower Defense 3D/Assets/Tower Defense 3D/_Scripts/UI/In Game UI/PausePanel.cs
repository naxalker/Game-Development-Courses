using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _buttonsHolder;
    [SerializeField] private GameObject _settingsPanel;

    private void OnEnable()
    {
        SwitchToPausePanel();

        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void SwitchToPausePanel()
    {
        _buttonsHolder.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void SwitchToSettingsPanel()
    {
        _buttonsHolder.SetActive(false);
        _settingsPanel.SetActive(true);
    }
}
