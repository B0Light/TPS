using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")]
    [SerializeField] protected float gravityForce = -40f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 0.3f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20;
    [SerializeField] protected float fallStratYVelocity = -5;
    protected bool fallingVelocityHAsBeenSet = false;
    protected float inAirTimer = 0;

    [Header("Flags")]
    public bool isRolling = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if(isGrounded)
        {
            if(yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHAsBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if(!character.characterNetworkManager.isJumping.Value && !fallingVelocityHAsBeenSet)
            {
                fallingVelocityHAsBeenSet = true;
                yVelocity.y = fallStratYVelocity;
            }

            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);
            yVelocity.y += gravityForce * Time.deltaTime;
        }

        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer); 
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(this.transform.position, groundCheckSphereRadius);
    }

    public void EnableCanRotate()
    {
        canRotate = true;
    }
    
    public void DisableCanRotate()
    {
        canRotate = false;
    }
}

