using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterEffects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstanceCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Final Damage")]
    public float finalDamageDealt = 0;  

    [Header("Poision")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;      

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;    
    
    public override void ProcessEffect(CharacterManager character)
    {
        if(character.characterNetworkManager.isInvulnerable.Value) return;
        
        base.ProcessEffect(character);

        if (character.isDead.Value)
            return;

        CaluclateDamage(character);
        PlayDirectionalBasedDamagedAnimation(character);
        // check for build up (poision, bleed ...)
        PlayDamageSFX(character);
        PlayDamageVFX(character);

    }

    private void CaluclateDamage(CharacterManager character)
    {
        if (!character.IsOwner) return;

        if(characterCausingDamage != null)
        {

        }
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }


        Debug.Log("FINAL DAMAGE GIVEN : " + finalDamageDealt);
        character.characterNetworkManager.currentHealth.Value -= (int)finalDamageDealt;

    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance.physicalDamageSFX);

        if(physicalDamageSFX != null)
            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
        character.characterSoundFXManager.PlayDamageGrunt();
        
        
    }

    private void PlayDirectionalBasedDamagedAnimation(CharacterManager character)
    {
        if (!character.IsOwner) return;

        if (character.isDead.Value) return;
        // TODO
        poiseIsBroken = true;



        if (145 <= angleHitFrom && angleHitFrom <= 180)
        {
            //front
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (-145 >= angleHitFrom && angleHitFrom >= -180)
        {
            // front
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (-45 <= angleHitFrom && angleHitFrom <= 45)
        {
            // back
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
        }
        else if (-144 <= angleHitFrom && angleHitFrom <= -45)
        {
            // left
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
        }
        else if (45 <= angleHitFrom && angleHitFrom <= 144)
        {
            // right
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
        }

        if(poiseIsBroken)
        {
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }
}
