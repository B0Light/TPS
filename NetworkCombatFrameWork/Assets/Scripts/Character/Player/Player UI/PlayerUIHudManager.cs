using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIHudManager : MonoBehaviour
{
    [Header("STAT BARS")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;

    [Header("QUICK SLOT")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }

    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("ITEM IS NULL");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if(weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        rightWeaponQuickSlotIcon.enabled = true;
        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("ITEM IS NULL");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        leftWeaponQuickSlotIcon.enabled = true;
        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
    }
}
