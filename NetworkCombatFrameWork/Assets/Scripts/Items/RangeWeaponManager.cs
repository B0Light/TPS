using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponManager : WeaponManager
{

    public WeaponController weaponController;

    protected override void Awake()
    {
        weaponController = GetComponentInChildren<WeaponController>();
    }

    public override void SetWeaponDamage(CharacterManager characterWieldingWeapon ,WeaponItem weapon)
    {
        weaponController.physicalDamage = weapon.physicalDamgae;
        weaponController.magicDamage = weapon.magicDamage;
        weaponController.fireDamage = weapon.fireDamage;
        weaponController.lightningDamage = weapon.lightningDamage;
        weaponController.holyDamage = weapon.holyDamage;
    }
}
