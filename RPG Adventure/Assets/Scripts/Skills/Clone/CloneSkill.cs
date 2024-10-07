using System.Collections;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private CloneSkillController _clonePrefab;
    [SerializeField] private float _cloneDuration;

    [Space]
    [SerializeField] private bool _canAttack;

    [SerializeField] private bool _createCloneOnDashStart;
    [SerializeField] private bool _createCloneOnDashOver;
    [SerializeField] private bool _createCloneOnCounterAttack;

    [Header("Duplicate")]
    [SerializeField] private bool _canDuplicateClone;
    [SerializeField] private float _chanceToDuplicate;

    [Header("Crystal Instead Of Clone")]
    public bool CrystalInsteadOfClone;

    public void CreateClone(Transform createPosition, Vector3 offset)
    {
        if (CrystalInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

        CloneSkillController newClone = Instantiate(_clonePrefab);
        newClone.Initialize(createPosition, _cloneDuration, _canAttack, offset, FindClosestEnemy(newClone.transform), _canDuplicateClone, _chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (_createCloneOnDashStart)
        {
            CreateClone(Player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (_createCloneOnDashOver)
        {
            CreateClone(Player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (_createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(enemyTransform, new Vector3(2 * Player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(.4f);

        CreateClone(transform, offset);
    }
}
