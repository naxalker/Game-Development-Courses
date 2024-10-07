using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private float _crystalDuration;
    [SerializeField] private CrystalSkillController _crystalPrefab;
    private CrystalSkillController _currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool _cloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool _canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private bool _canUseMultiStacks;
    [SerializeField] private int _stacksAmount;
    [SerializeField] private float _multiStackCooldown;
    [SerializeField] private float _useTimeWindow;
    [SerializeField] private List<CrystalSkillController> _crystalLeft;

    public override void UseSkill()
    {
        base.UseSkill();

        if (UseMultiCrystal())
            return;

        if (_currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (_canMoveToEnemy)
                return;

            Vector2 playerPos = Player.transform.position;
            Player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPos;

            if (_cloneInsteadOfCrystal)
            {
                SkillManager.Instance.Clone.CreateClone(_currentCrystal.transform, Vector3.zero);
                Destroy(_currentCrystal.gameObject);
            }
            else
            {
                _currentCrystal.FinishCrystal();
            }

            _currentCrystal.FinishCrystal();
        }
    }

    public void CreateCrystal()
    {
        _currentCrystal = Instantiate(_crystalPrefab, Player.transform.position, Quaternion.identity);

        _currentCrystal.Initialize(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, FindClosestEnemy(_currentCrystal.transform));
        _currentCrystal.ChooseRandomEnemy();
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        _currentCrystal.ChooseRandomEnemy();
    }

    private void RefilCrystal()
    {
        int amountToAdd = _stacksAmount - _crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            _crystalLeft.Add(_crystalPrefab);
        }
    }

    private bool UseMultiCrystal()
    {
        if (_canUseMultiStacks)
        {
            if (_crystalLeft.Count > 0)
            {
                if (_crystalLeft.Count == _stacksAmount)
                {
                    Invoke("ResetAbility", _useTimeWindow);
                }

                CooldownTimer = 0;
                CrystalSkillController crystalToSpawn = _crystalLeft[_crystalLeft.Count - 1];
                CrystalSkillController newCrystal = Instantiate(crystalToSpawn, Player.transform.position, Quaternion.identity);

                _crystalLeft.Remove(crystalToSpawn);

                newCrystal.Initialize(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (_crystalLeft.Count <= 0)
                {
                    CooldownTimer = _multiStackCooldown;
                    RefilCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void ResetAbility()
    {
        if (CooldownTimer > 0)
            return;

        CooldownTimer = _multiStackCooldown;
        RefilCrystal();
    }
}
