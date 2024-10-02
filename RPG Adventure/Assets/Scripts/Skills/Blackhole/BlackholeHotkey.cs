using TMPro;
using UnityEngine;

public class BlackholeHotkey : MonoBehaviour
{
    private KeyCode _key;
    private TMP_Text _text;

    private Transform _enemy;
    private BlackholeSkillController _blackhole;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _blackhole.AddEnemyToList(_enemy);

            _text.color = Color.clear;
        }
    }

    public void Initialize(KeyCode key, Transform enemy, BlackholeSkillController blackhole)
    {
        _enemy = enemy;
        _blackhole = blackhole;

        _key = key;
        _text.text = _key.ToString();
    }
}
