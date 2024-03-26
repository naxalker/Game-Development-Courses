using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Tweener : MonoBehaviour
    {
        [SerializeField] private float _delay;

        private void OnEnable()
        {
            LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.001f);
            LeanTween.scale(gameObject, new Vector3(.3f, .3f, .3f), 0.5f)
                .setDelay(_delay)
                .setEase(LeanTweenType.easeInOutCirc)
                .setLoopPingPong();
        }
    }
}
