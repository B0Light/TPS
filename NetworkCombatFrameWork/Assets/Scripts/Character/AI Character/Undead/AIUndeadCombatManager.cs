using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUndeadCombatManager : AICharacterCombatManager
{
   [Header("Damage Collider")]
   [SerializeField] private UndeadHandDamageCollider[] leftHandDamageColliders;
   [SerializeField] private UndeadHandDamageCollider[] rightHandDamageColliders;
   

   [Header("Damage")] 
   [SerializeField] private int baseDamage = 25;
   [SerializeField] private float attack01DamageModifier = 1.0f;
   [SerializeField] private float attack02DamageModifier = 1.4f;

   public void SetAttack01Damage()
   {
      foreach (var leftHandDamageCollider in leftHandDamageColliders)
      {
         leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
      }

      foreach (var rightHandDamageCollider in rightHandDamageColliders)
      {
         rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
      }
   }
   
   public void SetAttack02Damage()
   {
      foreach (var leftHandDamageCollider in leftHandDamageColliders)
      {
         leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
      }

      foreach (var rightHandDamageCollider in rightHandDamageColliders)
      {
         rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
      }
   }
   
   public void OpenLeftHandDamageCollier()
   {
      aiCharacter.characterSoundFXManager.PlayAttackGrunt();   
      
      foreach (var leftHandDamageCollider in leftHandDamageColliders)
      {
         leftHandDamageCollider.EnableDamageCollider();
      }
   }

   public void OpenRightHandDamageCollier()
   {
      aiCharacter.characterSoundFXManager.PlayAttackGrunt();   
      
      foreach (var rightHandDamageCollider in rightHandDamageColliders)
      {
         rightHandDamageCollider.EnableDamageCollider();
      }
   }
   
   public void CloseLeftHandDamageCollier()
   {
      foreach (var leftHandDamageCollider in leftHandDamageColliders)
      {
         leftHandDamageCollider.DisableDamageCollider();
      }
   }
   
   public void CloseRightHandDamageCollier()
   {
      foreach (var rightHandDamageCollider in rightHandDamageColliders)
      {
         rightHandDamageCollider.DisableDamageCollider();
      }
   }
   
   
}
