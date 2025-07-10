using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private BuildButtonsHolder _buildButtonsHolder;

    private void OnEnable()
    {
        ShowGameplayPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _buildButtonsHolder.IsActive == false)
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
