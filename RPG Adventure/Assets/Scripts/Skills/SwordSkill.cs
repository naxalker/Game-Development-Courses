using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private SwordSkillController _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravity;

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
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //_finalDir = new Vector2(AimDirection().normalized.x * _launchForce.x, AimDirection().normalized.y * _launchForce.y);
            _finalDir = AimDirection().normalized * _launchForce;
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
        newSword.Initialize(_finalDir, _swordGravity);

        DotsActive(false);
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(isActive);
        }
    }

    private Vector2 AimDirection()
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
            AimDirection().normalized * _launchForce * t + 
            .5f * (Physics2D.gravity * _swordGravity) * (t * t);

        return position;
    }
}
