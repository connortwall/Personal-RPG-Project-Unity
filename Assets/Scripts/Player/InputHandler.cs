using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CW
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool aInput;
        public bool b_input;
        public bool y_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool critical_Attack_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;

        public bool rightStick_Right_Input;
        public bool rightStick_Left_Input;
        
        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;
        
        // need a specific transform for critical attack (or else raycast will come from ground (default))
        public Transform criticalAttackRaycatStartPoint;
        
        private Vector2 cameraInput;

        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private WeaponSlotManager weaponSlotManager;
        private CameraHandler cameraHandler;
        private AnimatorManager animatorManager;
        private UIManager uIManager;

        private Vector2 movementInput;
        
        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            // calling this bc we must reload weapons on addition of new wepon
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorManager = GetComponentInChildren<AnimatorManager>();
            uIManager = FindObjectOfType<UIManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                inputActions.PlayerActions.SelectButton.performed += inputActions => aInput = true;
                
                // handle attack input
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                
                // handle quick slot
                inputActions.PlayerActions.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerActions.DPadLeft.performed += i => d_Pad_Left = true;
                
                // handle select
                inputActions.PlayerActions.SelectButton.performed += i => aInput = true;
                
                // handle roll input
                // when input is cancelled
                inputActions.PlayerActions.Roll.performed += i => b_input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_input = false;
                
                // handle jump input
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                
                //handle inventory input
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                
                // handle lock on input
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                
                //handle lock on right and left
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => rightStick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => rightStick_Left_Input = true;

                //handle 
                inputActions.PlayerActions.Y.performed += i => y_Input = true;

                // handle critical attack
                inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;

                Debug.DrawRay(criticalAttackRaycatStartPoint.position,transform.TransformDirection(Vector3.forward), Color.red, 0.5f);

            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
 
        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();
        }
        
        public void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            // will detect when key is pressed and make bool true
            //b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            //b_input = inputActions.PlayerActions.Roll.IsPressed();

            if (b_input)
            {
                rollInputTimer += delta;
                if (playerStats.currentStamina <= 0)
                {
                    b_input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;
                
                // tapping b
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                //reset the timer
                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            if (rb_Input == true)
            {
                playerAttacker.HandleRBAction();
            }
            if (rt_Input == true)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }
        
        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                if (inventoryFlag)
                {
                    uIManager.OpenSelectWindow();
                    uIManager.UpdateUI();
                    uIManager.hudWindow.SetActive(false);
                }
                else
                {
                    uIManager.CloseSelectWindow();
                    uIManager.CloseAllInventoryWindows();
                    uIManager.hudWindow.SetActive(true);
                }
            }

        }

        private void HandleLockOnInput()
        {
            // if no target is currently locked on
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                // find nearest lock on target to move camer to
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && rightStick_Left_Input)
            {
                rightStick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    // assign current lock on target to target in left
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            else if (lockOnFlag && rightStick_Right_Input)
            {
                rightStick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    // assign current lock on target to target in right
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                // switch to false so that it activates only once per frame
                y_Input = false;
                // switch state of flag
                twoHandFlag = !twoHandFlag;
                if (twoHandFlag)
                {
                    //enable two handed w. right weapon
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    //disable two handed
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

                }
            }
        }

        private void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                // disable after use
                critical_Attack_Input = false;
                playerAttacker.AttemptBackstabOrRiposte();
            }
        }
    }
}