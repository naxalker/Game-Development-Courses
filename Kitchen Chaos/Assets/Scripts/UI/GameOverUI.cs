using TMPro;
using UnityEngine;
using Zenject;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _recipesDeliveredAmountLabel;

    private GameManager _gameManager;
    private DeliveryManager _deliveryManager;

    [Inject]
    private void Construct(GameManager gameManager, DeliveryManager deliveryManager)
    {
        _gameManager = gameManager;
        _deliveryManager = deliveryManager;
    }

    private void Start()
    {
        _gameManager.StateChanged += StateChangedHandler;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameManager.StateChanged -= StateChangedHandler;
    }

    private void StateChangedHandler(GameManager.State state)
    {
        if (state == GameManager.State.GameOver)
        {
            _recipesDeliveredAmountLabel.text = _deliveryManager.SuccessfulRecipesAmount.ToString();

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
