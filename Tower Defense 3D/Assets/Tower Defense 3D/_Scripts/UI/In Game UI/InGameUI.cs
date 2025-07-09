using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameplayPanel;

    private void OnEnable()
    {
        ShowGameplayPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pausePanel.gameObject.activeSelf)
            {
                ShowGameplayPanel();
            }
            else
            {
                ShowPausePanel();
            }
        }
    }

    public void ShowGameplayPanel()
    {
        _pausePanel.SetActive(false);
        _gameplayPanel.SetActive(true);
    }

    public void ShowPausePanel()
    {
        _pausePanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }
}
