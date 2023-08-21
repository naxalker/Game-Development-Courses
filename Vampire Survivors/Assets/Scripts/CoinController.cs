using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;
    public int currentCoin;
    public CoinPickup coin;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoins(int coinsToAdd)
    {
        currentCoin += coinsToAdd;

        UIController.Instance.UpdateCoins();

        SFXManager.instance.PlaySFXPitched(2);
    }

    public void DropCoin(Vector3 position, int value)
    {
        CoinPickup newCoin = Instantiate(coin, position + new Vector3(.2f, .1f, 0f), Quaternion.identity);
        newCoin.coinAmount = value;
        newCoin.gameObject.SetActive(true);
    }

    public void SpendCoins(int coinsToSpend) 
    {
        currentCoin -= coinsToSpend;
        UIController.Instance.UpdateCoins();
    }
}
