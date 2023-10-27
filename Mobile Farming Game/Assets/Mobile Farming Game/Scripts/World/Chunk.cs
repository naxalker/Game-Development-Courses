using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private MeshFilter chunkFilter;
    private ChunkWalls chunkWalls;

    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool isUnlocked;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;

    private void Awake()
    {
        chunkWalls = GetComponent<ChunkWalls>();
    }

    private void Start()
    {
        currentPrice = initialPrice;
        priceText.text = currentPrice.ToString();
    }

    public void TryUnlock()
    {
        if (CashManager.instance.GetCoins() <= 0) return;

        currentPrice--;
        CashManager.instance.UseCoins(1);

        onPriceChanged?.Invoke();

        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
        {
            Unlock();
        }
    }

    private void Unlock(bool triggerAction = true)
    {
        unlockedElements.SetActive(true);
        lockedElements.SetActive(false);

        isUnlocked = true;
        
        if (triggerAction)
            onUnlocked?.Invoke();
    }

    public void UpdateWalls(int configuration)
    {
        chunkWalls.Configure(configuration);
    }

    public void DisplayLockedElements()
    {
        lockedElements.SetActive(true);
    }

    public void Initialize(int loadedPrice)
    {
        currentPrice = loadedPrice;
        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            Unlock(false);
    }

    public void SetRenderer(Mesh chunkMesh)
    {
        chunkFilter.mesh = chunkMesh;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    public int GetInitialPrice()
    {
        return initialPrice;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
