using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    [Header("WeaponType")] public WeaponType weaponType;
    
    [Header("Weapon Model")] public GameObject weaponModel;

    [Header("Weapon Requirements")] public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")] public int physicalDamgae = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    [Header("Weapon Base Poise Damage")] public float poiseDamage = 10;

    [Header("Weapon Modifiers")] public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.2f;
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;
    public float charge_Attack_01_Modifier = 2.0f;
    public float charge_Attack_02_Modifier = 3.0f;

    [Header("Stamina Costs Modifiers")] public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 0.9f;

    [Header("Actions")] 
    public WeaponItemAction oh_LightAttack_Action; //one hand right bumper
    public WeaponItemAction oh_HeavyAttack_Action;

    [Header("SFX")] 
    public AudioClip[] soundEffects;

}
