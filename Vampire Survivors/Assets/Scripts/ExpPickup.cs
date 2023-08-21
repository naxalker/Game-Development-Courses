using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float timeBetweenChecks;

    private bool movingToPlayer;
    private float checkCounter;
    private PlayerController player;

    public int expValue;

    private void Start()
    {
        player = PlayerHealthController.Instance.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (movingToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        } else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;

                if (Vector3.Distance(transform.position, player.transform.position) < player.pickupRange)
                {
                    movingToPlayer = true;
                    moveSpeed += player.moveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceLevelController.Instance.GetExp(expValue);

            Destroy(gameObject);
        }
    }
}
