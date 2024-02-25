using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Zoom Action")]
public class RangeWeaponItemZoomAction : WeaponItemAction
{
    [SerializeField] private WeaponController activeWeapon;
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);
        
        if (!playerPerformingAction.IsOwner) return;
        
        GetActiveWeapon(playerPerformingAction);
        
        if(activeWeapon)
            Zoom(playerPerformingAction);
    }

    private void Zoom(PlayerManager playerManager)
    {
        playerManager.IsAiming = !playerManager.IsAiming;
        PlayerCamera.Instance.Aiming();
    }

    private void GetActiveWeapon(PlayerManager playerManager)
    {
        if(activeWeapon != null) return;
        
        activeWeapon = playerManager.playerEquipmentManger.rightHandWeaponModel.GetComponentInChildren<WeaponController>();
        
    }
}
