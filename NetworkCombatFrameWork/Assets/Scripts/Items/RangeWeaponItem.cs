using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapons/Range Weapon ")]
public class RangeWeaponItem : WeaponItem
{
    public struct CrosshairData
    {
        public Sprite CrosshairSprite;
        public int CrosshairSize;
        public Color CrosshairColor;
    }
    
    public CrosshairData CrosshairDataDefault;
    public CrosshairData CrosshairDataTargetInSight;
}
