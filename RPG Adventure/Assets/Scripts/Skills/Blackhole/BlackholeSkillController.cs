using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    public bool PlayerCanExitState { get; private set; }
    private bool _playerCanDisappear = true;

    [Header("Blackhole")]
    private bool _canGrow = true;
    private bool _canShrink;
    private float _maxSize;
    private float _growSpeed;
    private float _shrinkSpeed;
    private float _blackholeTimer;

    [Header("Clones")]
    private bool _cloneAttackReleased;
    private int _attacksAmount;
    private float _cloneAttackCooldown;
    private float _cloneAttackTimer;
    private List<Transform> _targets = new List<Transform>();

    [Header("Hotkeys")]
    [SerializeField] private BlackholeHotkey _hotkeyPrefab;
    [SerializeField] private List<KeyCode> _keyCodes;
    private bool _canCreateHotkeys = true;
    private List<BlackholeHotkey> _hotkeys = new List<BlackholeHotkey>();

    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime;
        _blackholeTimer -= Time.deltaTime;

        if (_blackholeTimer < 0)
        {
            _blackholeTimer = Mathf.Infinity;

            if (_targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CreateClones();

        if (_canGrow && !_canShrink)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        }

        if (_canShrink)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeTime(true);
            CreateHotkey(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeTime(false);
        }
    }

    public void Initialize(float maxSize, float growSpeed, float shrinkSpeed, int attacksAmount, float cloneAttackCooldown, float blackholeDuration)
    {
        _maxSize = maxSize;
        _growSpeed = growSpeed;
        _shrinkSpeed = shrinkSpeed;
        _attacksAmount = attacksAmount;
        _cloneAttackCooldown = cloneAttackCooldown;
        _blackholeTimer = blackholeDuration;

        if (SkillManager.Instance.Clone.CrystalInsteadOfClone)
            _playerCanDisappear = false;
    }

    public void AddEnemyToList(Transform enemyTransform)
    {
        _targets.Add(enemyTransform);
    }

    private void CreateClones()
    {
        if (_cloneAttackTimer < 0 && _cloneAttackReleased && _attacksAmount > 0)
        {
            _cloneAttackTimer = _cloneAttackCooldown;

            float xOffset = Random.Range(0, 100) > 50 ? 2 : -2;

            if (SkillManager.Instance.Clone.CrystalInsteadOfClone)
            {
                SkillManager.Instance.Crystal.CreateCrystal();
                SkillManager.Instance.Crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.Instance.Clone.CreateClone(_targets[Random.Range(0, _targets.Count)], new Vector3(xOffset, 0));
            }

            _attacksAmount--;

            if (_attacksAmount <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkeys();
        PlayerCanExitState = true;
        _canShrink = true;
        _cloneAttackReleased = false;
    }

    private void ReleaseCloneAttack()
    {
        if (_targets.Count <= 0)
            return;

        DestroyHotkeys();
        _cloneAttackReleased = true;
        _canCreateHotkeys = false;

        if (_playerCanDisappear)
        {
            PlayerManager.Instance.Player.MakeTransparent(true);
            _playerCanDisappear = false;
        }
    }

    private void CreateHotkey(Enemy enemy)
    {
        if (_keyCodes.Count <= 0)
            return;

        if (!_canCreateHotkeys)
            return;

        BlackholeHotkey newHotkey = Instantiate(_hotkeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
        _hotkeys.Add(newHotkey);

        KeyCode chosenKey = _keyCodes[Random.Range(0, _keyCodes.Count)];
        _keyCodes.Remove(chosenKey);

        newHotkey.Initialize(chosenKey, enemy.transform, this);
    }

    private void DestroyHotkeys()
    {
        if (_hotkeys.Count <= 0)
            return;

        for (int i = 0; i < _hotkeys.Count; i++)
        {
            Destroy(_hotkeys[i].gameObject);
        }
    }
}
