using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Shooting Action")]
public class RangeWeaponItemShootingAction : WeaponItemAction
{
    [SerializeField] private WeaponController activeWeapon;
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);
        
        if (!playerPerformingAction.IsOwner) return;
        GetActiveWeapon(playerPerformingAction);
        
        if(activeWeapon)
            Shooting();
    }

    private void Shooting()
    {
        if (activeWeapon.IsReloading)
            return;
        
        bool hasFired = activeWeapon.HandleShootInputs(true,true,false);
    }

    private void GetActiveWeapon(PlayerManager playerManager)
    {
        if(activeWeapon != null) return;
        
        activeWeapon = playerManager.playerEquipmentManger.rightHandWeaponModel.GetComponentInChildren<WeaponController>();
        
    }
}
