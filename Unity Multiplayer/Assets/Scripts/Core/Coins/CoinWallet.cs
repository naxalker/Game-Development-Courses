using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            int coinValue = coin.Collect();

            if (IsServer)
            {
                TotalCoins.Value += coinValue;
            }
        }
    }

    public void SpendCoins(int coinsAmount)
    {
        TotalCoins.Value -= coinsAmount;
    }
}
