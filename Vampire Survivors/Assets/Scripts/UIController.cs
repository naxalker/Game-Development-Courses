using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] Slider expLvlSlider;
    [SerializeField] TMP_Text expLvlText;
    [SerializeField] TMP_Text coinText;
    
    public static UIController Instance;
    public LevelUpSelectionButton[] levelUpButtons;
    public GameObject levelUpPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateExperience(int currentExp, int levelExp, int currentLvl)
    {
        expLvlSlider.maxValue = levelExp;
        expLvlSlider.value = currentExp;

        expLvlText.text = "Level: " + currentLvl; 
    }

    public void SkipLevelUp()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateCoins()
    {
        coinText.text = "Coins: " + CoinController.instance.currentCoin;
    }
}
