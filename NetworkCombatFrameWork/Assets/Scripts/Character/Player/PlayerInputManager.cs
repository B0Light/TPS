 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    // LOCAL PLAYER
    public PlayerManager player;

    PlayerControls playerControls;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("Lock On")]
    [SerializeField] bool lockOnInput;
    [SerializeField] bool lockOn_LeftInput;
    [SerializeField] bool lockOn_RightInput;
    private Coroutine lockOnCoroutine;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool switchLWeapon = false;
    [SerializeField] bool switchRWeapon = false;
    

    [Header("BUMPER INPUT")]
    [SerializeField] bool lightAttackInput = false;

    [Header("TRIGGER INPUT")]
    [SerializeField] bool heavyAttackInput = false;
    [SerializeField] bool chargeAttackInput = false;
    
    [Header("RANGE INPUT")]
    [SerializeField] bool fire1Input = false;
    [SerializeField] bool fire2Input = false;
    [SerializeField] bool zoomInput = false;
    [SerializeField] bool reload = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // WHEN THE SCENES CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += OnSceneChange;

        Instance.enabled = false;
        if(playerControls != null )
            playerControls.Disable();
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE OUR PLAYERS CONTROLS
        if(newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {
            Instance.enabled = true;

            if (playerControls != null)
                playerControls.Enable();
        }
        // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYERS CONTROLS
        // THIS IS SO OUR PLAYER CAN'T MOVE AROUND IF WE ENTER THINGS LIKE A CHARACTER CREAETION MENU ETC...
        else
        {
            Instance.enabled = false;
            if (playerControls != null)
                playerControls.Disable();
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

            // ACTIONS
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.SwitchLWeapon.performed += i => switchLWeapon = true;
            playerControls.PlayerActions.SwitchRWeapon.performed += i => switchRWeapon = true;

            // BUMPER
            playerControls.PlayerActions.LightAttack.performed += i => lightAttackInput = true;

            // TRIGGER
            playerControls.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;
            playerControls.PlayerActions.ChargeAttack.performed += i => chargeAttackInput = true;
            playerControls.PlayerActions.ChargeAttack.canceled += i => chargeAttackInput = false;

            // LOCK ON
            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_LeftInput = true;
            playerControls.PlayerActions.SeekRightLockOnTarget1.performed += i => lockOn_RightInput = true;

            // HOLDING THE INPUT, SETS THE BOOL TO TRUE
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            // RELEASING THE INPUT, SETS THE BOOL TO FALSE
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            
            // Range
            playerControls.PlayerActions.Reload.performed += i => reload = true;
            playerControls.PlayerActions.Fire1.performed += i => fire1Input = true;
            playerControls.PlayerActions.Fire2.performed += i => fire2Input = true;
            playerControls.PlayerActions.Fire2.canceled += i => fire2Input = false;
            
            playerControls.PlayerActions.Zoom.performed += i => zoomInput = true;
            playerControls.PlayerActions.Zoom.canceled += i => zoomInput = false;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // IF WE MINIMIZE OR LOWER THE WINDOW, STOP ADJUSTING INPUTS
    private void OnApplicationFocus(bool focus)
    {
        if(enabled)
        {
            if(focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleLightAttackInput();
        HandleHeavyAttackInput();
        HandleChargeAttackInput();
        HandleSwitchLWeaponInput();
        HandleSwitchRWeaponInput();
        
        //range
        HandleFireInput();
        HandleZoomInput();
        HandleReloadInput();
    }

    //lock on
    private void HandleLockOnInput()
    {
        if(player.playerNetworkManager.isLockOn.Value)
        {
            if (player.playerCombatManager.currentTarget == null)
                return;

            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetworkManager.isLockOn.Value = false;
            }

            if(lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitThenFindNewTarget());
        }

        if(lockOnInput && player.playerNetworkManager.isLockOn.Value)
        {
            lockOnInput = false;
            PlayerCamera.Instance.ClearLockOnTarget();
            player.playerNetworkManager.isLockOn.Value = false;
            return;
        }

        if (lockOnInput && !player.playerNetworkManager.isLockOn.Value)
        {
            lockOnInput = false;
            PlayerCamera.Instance.HandleLocatingLockOnTarget();

            if(PlayerCamera.Instance.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.Instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockOn.Value = true;
            }
        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOn_LeftInput)
        {
            lockOn_LeftInput = false;

            if (player.playerNetworkManager.isLockOn.Value)
            {
                PlayerCamera.Instance.HandleLocatingLockOnTarget();

                if(PlayerCamera.Instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.Instance.leftLockOnTarget);
                }
            }
        }

        if (lockOn_RightInput)
        {
            lockOn_RightInput = false;

            if (player.playerNetworkManager.isLockOn.Value)
            {
                PlayerCamera.Instance.HandleLocatingLockOnTarget();

                if (PlayerCamera.Instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.Instance.rightLockOnTarget);
                }
            }
        }
    }

    // MOVEMENT

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        // RETURNS THE ABSOLUTE NUMBER, (meaning number without the negative sign, so it's always positive)
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        // WE CLAMP THE VALUES, SO THEY ARE 0, 0.5 OR 1 (OPTIONAL)
        if(moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON-STRAFING MOVEMENT
        // WE USE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

        if (player == null)
            return;

        if (moveAmount != 0)
        {
            player.playerNetworkManager.isMoving.Value = true;
        }
        else
        {
            player.playerNetworkManager.isMoving.Value = false;
        }

        // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT

        if (!player.playerNetworkManager.isLockOn.Value || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
        }


        // IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT AS WELL
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    // ACTION

    private void HandleDodgeInput()
    {
        if(dodgeInput)
        {
            dodgeInput = false;

            // FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintInput()
    {
        if(sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            // IF WE HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHING

            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    private void HandleLightAttackInput()
    {
        if(lightAttackInput)
        {
            lightAttackInput = false;

            player.playerNetworkManager.SetCharacterAcionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(
                player.playerInventoryManager.currentRightHandWeapon.oh_LightAttack_Action,
                player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleHeavyAttackInput()
    {
        if (heavyAttackInput)
        {
            heavyAttackInput = false;

            player.playerNetworkManager.SetCharacterAcionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(
                player.playerInventoryManager.currentRightHandWeapon.oh_HeavyAttack_Action,
                player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleChargeAttackInput()
    {
        if(player.isPerformingAction)
        {
            if(player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerNetworkManager.isChargingAttack.Value = chargeAttackInput;
            }
        }
        
    }

    private void HandleSwitchLWeaponInput()
    {
        if (switchLWeapon)
        {
            switchLWeapon = false;
            player.playerEquipmentManger.SwithLeftWeapon();
        }
    }

    private void HandleSwitchRWeaponInput()
    {
        if (switchRWeapon)
        {
            switchRWeapon = false;
            player.playerEquipmentManger.SwithRightWeapon();
        }
    }

    private void HandleFireInput()
    {
        if(fire1Input)
        {
            fire1Input = false;

            player.playerNetworkManager.SetCharacterAcionHand(true);

            player.playerWeaponManager.Shooting(fire1Input,fire2Input,fire2Input);
        }
    }

    private void HandleReloadInput()
    {
        if (reload)
        {
            reload = false;
            player.playerWeaponManager.Reloading();
        }
    }

    private void HandleZoomInput()
    {
        if(zoomInput)
        {
            player.playerWeaponManager.IsAiming = true;
        }
        else
        {
            player.playerWeaponManager.IsAiming = false;
        }
        
    }
}

