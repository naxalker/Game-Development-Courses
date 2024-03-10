using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    protected int CoinValue = 10;
    protected bool AlreadyCollected;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    public abstract int Collect();

    public void SetValue(int value)
    {
        CoinValue = value;
    }

    protected void Show(bool show)
    {
        _spriteRenderer.enabled = show;
    }
}
