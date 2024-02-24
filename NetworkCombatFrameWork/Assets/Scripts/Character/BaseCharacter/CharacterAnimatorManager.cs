using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;
    
    [Header("Flags")]
    public bool applyRootMotion = false;

    [Header("Damaged Animation")]
    public string lastDamageAnimationPlayed;

    [SerializeField] string hit_Forward_01  = "hit_Forward_Medium_01";
    [SerializeField] string hit_Forward_02  = "hit_Forward_Medium_02";

    [SerializeField] string hit_Backward_01 = "hit_Backward_Medium_01";
    [SerializeField] string hit_Backward_02 = "hit_Backward_Medium_02";

    [SerializeField] string hit_Left_01     = "hit_Left_Medium_01";
    [SerializeField] string hit_Left_02     = "hit_Left_Medium_02";

    [SerializeField] string hit_Right_01    = "hit_Right_Medium_01";
    [SerializeField] string hit_Right_02    = "hit_Right_Medium_02";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
        forward_Medium_Damage.Add(hit_Forward_01);
        forward_Medium_Damage.Add(hit_Forward_02);

        backward_Medium_Damage.Add(hit_Backward_01);
        backward_Medium_Damage.Add(hit_Backward_02);

        left_Medium_Damage.Add(hit_Left_01);
        left_Medium_Damage.Add(hit_Left_02);

        right_Medium_Damage.Add(hit_Right_01);
        right_Medium_Damage.Add(hit_Right_02);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();

        foreach (var item in animationList)
        {
            finalList.Add(item);
        }
        finalList.Remove(lastDamageAnimationPlayed);

        for(int i = finalList.Count - 1; i >= 0; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;

        if(0 < horizontalMovement && horizontalMovement <= 0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if(0.5f < horizontalMovement && horizontalMovement <= 1f)
        {
            snappedHorizontal = 1f;
        }
        else if (-0.5f <= horizontalMovement && horizontalMovement < 0f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (-1 <= horizontalMovement && horizontalMovement < -0.5f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0f;
        }

        if (0 < verticalMovement && verticalMovement <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (0.5f < verticalMovement && verticalMovement <= 1f)
        {
            snappedVertical = 1f;
        }
        else if (-0.5f <= verticalMovement && verticalMovement < 0f)
        {
            snappedVertical = -0.5f;
        }
        else if (-1f <= verticalMovement && verticalMovement < -0.5f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical  = 0f;
        }


        if (isSprinting)
        {
            snappedVertical = 2;
        }

        character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction, 
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false)
    {
        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
        // FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
        // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;

        // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(
        AttackType attackType,
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false)
    {
        // keep track last attack performed (for combo)
        // attack typt light, heavy, ect
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canMove = canMove;

        // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void EnableCanDoCombo()
    {
       
    }

    public virtual void DisableCanDoCombo()
    {

    }
}

