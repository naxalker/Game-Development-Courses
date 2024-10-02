using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private CloneSkillController _clonePrefab;
    [SerializeField] private float _cloneDuration;

    [Space]
    [SerializeField] private bool _canAttack;

    public void CreateClone(Transform createPosition, Vector3 offset)
    {
        CloneSkillController newClone = Instantiate(_clonePrefab);
        newClone.Initialize(createPosition, _cloneDuration, _canAttack, offset);
    }
}
