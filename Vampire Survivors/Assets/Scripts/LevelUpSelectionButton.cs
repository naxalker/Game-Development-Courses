using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionButton : MonoBehaviour
{
    [SerializeField] TMP_Text upgradeDescriptionText, nameLevelText;
    [SerializeField] Image weaponIcon;

    private Weapon assignedWeapon;

    public void UpdateButtonDisplay(Weapon theWeapon)
    {
        weaponIcon.sprite = theWeapon.icon;

        if (theWeapon.gameObject.activeSelf)
        {
            upgradeDescriptionText.text = theWeapon.stats[theWeapon.weaponLevel].upgradeText;
            nameLevelText.text = theWeapon.name + " - Lvl " + theWeapon.weaponLevel;
        } else
        {
            upgradeDescriptionText.text = "Unlock " + theWeapon.name;
            nameLevelText.text = theWeapon.name;
        }

        assignedWeapon = theWeapon;
    }

    public void SelectUpgrade()
    {
        if (assignedWeapon != null)
        {
            if(assignedWeapon.gameObject.activeSelf)
            {
                assignedWeapon.LevelUp();
            } else
            {
                PlayerController.Instance.AddWeapon(assignedWeapon);
            }
            

            UIController.Instance.levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
