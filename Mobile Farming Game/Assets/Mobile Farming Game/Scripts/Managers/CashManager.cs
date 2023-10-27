using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;

    [Header("Settings")]
    private int coins;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();

        UpdateCoinContainers();
    }

    public void AddCoins(int amount)
    {
        coins += amount;

        UpdateCoinContainers();

        SaveData();
    }

    public void UseCoins(int amount)
    {
        AddCoins(-amount);
    }

    public int GetCoins()
    {
        return coins;
    }

    [NaughtyAttributes.Button]
    private void Add500Coins()
    {
        AddCoins(500);
    }

    private void UpdateCoinContainers()
    {
        GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject coinContainer in coinContainers)
        {
            coinContainer.GetComponent<TextMeshProUGUI>().text = coins.ToString();
        }
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins");
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }
}
