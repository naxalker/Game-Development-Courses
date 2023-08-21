using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] float hitWaitTime = 1f;
    [SerializeField] float health = 5f;
    [SerializeField] float knockBackTime = .5f;
    [SerializeField] int expToGive = 1;
    [SerializeField] int coinValue = 1;
    [SerializeField] float coinDropRate = .5f;

    private Transform target;
    private Rigidbody2D rb;
    private float hitCounter;
    private float knockBackCounter;

    private void Awake()
    {
        target = PlayerHealthController.Instance.transform;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PlayerController.Instance.gameObject.activeSelf)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2;
                }

                if (knockBackCounter <= 0)
                {
                    moveSpeed = -moveSpeed * 0.5f;
                }
            }

            rb.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (hitCounter > 0f)
            {
                hitCounter -= Time.deltaTime;
            }
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.Instance.TakeDamage(damage);

            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Destroy(gameObject);

            ExperienceLevelController.Instance.SpawnExp(transform.position, expToGive);
            
            if (Random.value <= coinDropRate)
            {
                CoinController.instance.DropCoin(transform.position, coinValue);
            }

            SFXManager.instance.PlaySFXPitched(0);
        } else
        {
            SFXManager.instance.PlaySFXPitched(1);
        }

        DamageNumberController.Instance.SpawnDamage(damage, transform.position);
    }

    public void TakeDamage(float damage, bool shouldKnockBack)
    {
        TakeDamage(damage);

        if (shouldKnockBack)
        {
            knockBackCounter = knockBackTime;
        }
    }
}
