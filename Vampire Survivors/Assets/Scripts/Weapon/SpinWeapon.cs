using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : Weapon
{
    [SerializeField] float rotateSpeed;
    [SerializeField] Transform holder, fireballToSpawn;
    [SerializeField] float timeBetweenSpawn;
    [SerializeField] EnemyDamager damager;

    private float spawnCounter;

    private void Start()
    {
        SetStats();
    }

    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + rotateSpeed * Time.deltaTime * stats[weaponLevel].speed);

        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0 ) 
        {
            spawnCounter = timeBetweenSpawn;

            for (int i = 0; i < stats[weaponLevel].amount; i++)
            {
                float rotation = 360f / stats[weaponLevel].amount * i;

                Instantiate(fireballToSpawn, fireballToSpawn.position, Quaternion.Euler(0f, 0f, rotation), holder).gameObject.SetActive(true);
            }
        }

        if (statsUpdated)
        {
            statsUpdated = false;

            SetStats();
        }
    }

    public void SetStats()
    {
        damager.damage = stats[weaponLevel].damage;

        transform.localScale = Vector3.one * stats[weaponLevel].range;

        timeBetweenSpawn = stats[weaponLevel].timeBetweenAttacks;

        damager.lifeTime = stats[weaponLevel].duration;

        spawnCounter = 0f;
    }
}
