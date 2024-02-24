using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    protected virtual void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();

    }

    public virtual void SetWeaponDamage(CharacterManager characterWieldingWeapon ,WeaponItem weapon)
    {
        meleeDamageCollider.characterCasuingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamgae;
        meleeDamageCollider.magicDamage = weapon.magicDamage;
        meleeDamageCollider.fireDamage = weapon.fireDamage;
        meleeDamageCollider.lightningDamage = weapon.lightningDamage;
        meleeDamageCollider.holyDamage = weapon.holyDamage;

        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
        meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;

        meleeDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
        meleeDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;

        meleeDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
        meleeDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;
    }
}
