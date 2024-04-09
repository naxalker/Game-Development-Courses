using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event Action<int> PlatesAmountChanged;

    private const float SpawnPlateTimerMax = 4f;
    private const int PlatesSpawnedAmountMax = 4;

    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;

    private float _spawnPlateTimer;
    private int _platesSpawnedAmount;

    private void Update()
    {
        if (_platesSpawnedAmount == PlatesSpawnedAmountMax) return;

        _spawnPlateTimer += Time.deltaTime;

        if (_spawnPlateTimer >= SpawnPlateTimerMax)
        {
            _spawnPlateTimer = 0;

            if (_platesSpawnedAmount < PlatesSpawnedAmountMax)
            {
                _platesSpawnedAmount++;

                PlatesAmountChanged?.Invoke(_platesSpawnedAmount);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject == false)
        {
            if (_platesSpawnedAmount > 0)
            {
                _platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);

                PlatesAmountChanged?.Invoke(_platesSpawnedAmount);
            }
        }
    }
}
