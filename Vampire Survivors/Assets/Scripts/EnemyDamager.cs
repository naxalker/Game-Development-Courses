using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float damage;
    public float lifeTime;
    public float timeBetweenDamage;

    [SerializeField] float growSpeed = 5f;
    [SerializeField] bool shouldKnockBack;
    [SerializeField] bool destroyParent;
    [SerializeField] bool damageOverTime;
    [SerializeField] bool destroyOnImpact;

    private Vector3 targetSize;
    private float damageCounter;
    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    private void Start()
    {
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) 
        {
            targetSize = Vector3.zero;

            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);

                if (destroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if (damageOverTime)
        {
            damageCounter -= Time.deltaTime;

            if (damageCounter <= 0)
            {
                damageCounter = timeBetweenDamage;

                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] != null)
                    {
                        enemiesInRange[i].TakeDamage(damage, shouldKnockBack);
                    } else
                    {
                        enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageOverTime == false)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<EnemyController>().TakeDamage(damage, shouldKnockBack);

                if (destroyOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        } else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                enemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (damageOverTime)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }
}
