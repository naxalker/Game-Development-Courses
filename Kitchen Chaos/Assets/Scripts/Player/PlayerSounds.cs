using UnityEngine;
using Zenject;

public class PlayerSounds : MonoBehaviour
{
    private const float FootstepTimerMax = .1f;

    private SoundManager _soundManager;

    private Player _player;
    private float _footstepTimer;

    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0)
        {
            if (_player.IsWalking)
            {
                _soundManager.PlayFootstepSound(_player.transform.position);
            }

            _footstepTimer = FootstepTimerMax;
        }
    }
}
