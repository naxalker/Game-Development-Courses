using System;
using UnityEngine;

public class SwordSkill : Skill
{
    [SerializeField] private SwordType _swordType;

    [Header("Skill Info")]
    [SerializeField] private SwordSkillController _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravity;
    [SerializeField] private float _freezeTimeDuration;
    [SerializeField] private float _returnSpeed;

    [Header("Bounce Info")]
    [SerializeField] private int _bounceAmount;
    [SerializeField] private float _bounceGravity;
    [SerializeField] private float _bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private int _pierceAmount;
    [SerializeField] private float _pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float _hitCooldown = .35f;
    [SerializeField] private float _maxTravelDistance = 7f;
    [SerializeField] private float _spinDuration = 2f;
    [SerializeField] private float _spinGravity = 1f;

    [Header("Aim Dots")]
    [SerializeField] private int _numberOfDots;
    [SerializeField] private float _spaceBetweenDots;
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private Transform _dotsParent;

    private Vector2 _finalDir;
    private GameObject[] _dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _finalDir = GetAimDirection().normalized * _launchForce;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i].transform.position = DotsPosition(i  * _spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        SwordSkillController newSword = Instantiate(_swordPrefab, Player.transform.position, Quaternion.identity);

        if (_swordType == SwordType.Bounce)
        {
            newSword.SetupBounce(true, _bounceAmount, _bounceSpeed);
        }
        else if (_swordType == SwordType.Pierce)
        {
            newSword.SetupPierce(_pierceAmount);
        }
        else if (_swordType == SwordType.Spin)
        {
            newSword.SetupSpin(true, _maxTravelDistance, _spinDuration, _hitCooldown);
        }

        newSword.Initialize(_finalDir, _swordGravity, Player, _freezeTimeDuration, _returnSpeed);

        Player.AssignNewSword(newSword);

        ActivateDots(false);
    }

    public void ActivateDots(bool isActive)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(isActive);
        }
    }

    private void SetupGravity()
    {
        if (_swordType == SwordType.Bounce)
        {
            _swordGravity = _bounceGravity;
        }
        else if (_swordType == SwordType.Pierce)
        {
            _swordGravity = _pierceGravity;
        }
        else if (_swordType == SwordType.Spin)
        {
            _swordGravity = _spinGravity;
        }
    }

    private Vector2 GetAimDirection()
    {
        Vector2 playerPos = Player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - playerPos;

        return direction;
    }

    private void GenerateDots()
    {
        _dots = new GameObject[_numberOfDots];

        for (int i = 0; i < _numberOfDots; i++)
        {
            _dots[i] = Instantiate(_dotPrefab, Player.transform.position, Quaternion.identity, _dotsParent);
            _dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)Player.transform.position + 
            GetAimDirection().normalized * _launchForce * t + 
            .5f * (Physics2D.gravity * _swordGravity) * (t * t);

        return position;
    }
}
