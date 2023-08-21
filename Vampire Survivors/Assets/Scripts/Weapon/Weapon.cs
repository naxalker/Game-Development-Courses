using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponLevel;
    public Sprite icon;
    public List<WeaponStats> stats;

    [HideInInspector]
    public bool statsUpdated;

    public void LevelUp()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;

            statsUpdated = true;

            if (weaponLevel >= stats.Count - 1)
            {
                PlayerController.Instance.fullyLevelledWeapons.Add(this);
                PlayerController.Instance.assignedWeapons.Remove(this);
            }
        }
    }
}

[Serializable]
public class WeaponStats
{
    public float speed;
    public float damage;
    public float range;
    public float timeBetweenAttacks;
    public float amount;
    public float duration;
    public string upgradeText;
}
