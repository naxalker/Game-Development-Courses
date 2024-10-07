using UnityEngine;

public class BlackholeSkill : Skill
{
    [Header("Blackhole Settings")]
    [SerializeField] private BlackholeSkillController _blackholePrefab;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _shrinkSpeed;
    [SerializeField] private float _blackholeDuration;

    [Header("Clones Settings")]
    [SerializeField] private int _attacksAmount;
    [SerializeField] private float _cloneCooldown;

    private BlackholeSkillController _currentBlackhole;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill => base.CanUseSkill;

    public override void UseSkill()
    {
        base.UseSkill();

        _currentBlackhole = Instantiate(_blackholePrefab, Player.transform.position, Quaternion.identity);

        _currentBlackhole.Initialize(_maxSize, _growSpeed, _shrinkSpeed, _attacksAmount, _cloneCooldown, _blackholeDuration);
    }

    public bool SkillCompleted()
    {
        if (_currentBlackhole == null)
            return false;

        if (_currentBlackhole.PlayerCanExitState)
        {
            _currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetblackholeRadius() => _maxSize / 2;
}
