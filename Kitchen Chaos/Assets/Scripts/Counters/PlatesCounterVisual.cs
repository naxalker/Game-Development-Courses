using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private List<GameObject> _platesVisuals;

    private PlatesCounter _platesCounter;

    private void Awake()
    {
        _platesCounter = GetComponentInParent<PlatesCounter>();
    }

    private void OnEnable()
    {
        _platesCounter.PlatesAmountChanged += PlatesAmountChangedHandler;
    }

    private void OnDisable()
    {
        _platesCounter.PlatesAmountChanged -= PlatesAmountChangedHandler;
    }

    private void PlatesAmountChangedHandler(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _platesVisuals[i].SetActive(true);
        }

        for (int i = amount; i < _platesVisuals.Count; i++)
        {
            _platesVisuals[i].SetActive(false);
        }
    }
}
