public class EnemyStats : CharacterStats
{
    private Enemy _enemy;

    protected override void Start()
    {
        base.Start();

        _enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        _enemy.DamageEffect();
    }
}
