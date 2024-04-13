using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Animator))]
public class DeliveryResult : MonoBehaviour
{
    private const string Popup = "Popup";

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _messageLabel;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failSprite;

    private DeliveryManager _deliveryManager;
    private Animator _animator;

    [Inject]
    private void Construct(DeliveryManager deliveryManager)
    {
        _deliveryManager = deliveryManager;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.SetActive(false);

        _deliveryManager.RecipeSuccessed += RecipeSuccessedHandler;
        _deliveryManager.RecipeFailed += RecipeFailedHandler;
    }

    private void OnDestroy()
    {
        _deliveryManager.RecipeSuccessed -= RecipeSuccessedHandler;
        _deliveryManager.RecipeFailed -= RecipeFailedHandler;
    }

    private void RecipeSuccessedHandler()
    {
        gameObject.SetActive(true);

        _backgroundImage.color = _successColor;
        _iconImage.sprite = _successSprite;
        _messageLabel.text = "DELIVERY\nSUCCESS";

        _animator.SetTrigger(Popup);
    }

    private void RecipeFailedHandler()
    {
        gameObject.SetActive(true);

        _backgroundImage.color = _failColor;
        _iconImage.sprite = _failSprite;
        _messageLabel.text = "DELIVERY\nFAILED";

        _animator.SetTrigger(Popup);
    }
}
