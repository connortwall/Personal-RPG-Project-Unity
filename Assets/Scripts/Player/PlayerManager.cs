using UnityEngine;

namespace CW
{
    public class PlayerManager : CharacterManager
    {
        private Animator animator;
        private CameraHandler cameraHandler; 
        private InputHandler inputHandler;
        private PlayerStatsManager playerStatsManager;
        private PlayerFXManager playerFXManager;
        private PlayerAnimatorManager playerAnimatorManager;
        private PlayerLocomotionManager playerLocomotionManager;
        
        private InteractableUI interactableUI; 
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;


        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            backstabCollider = GetComponentInChildren<CriticalDamageCollider>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            interactableUI = FindObjectOfType<InteractableUI>();

        }
        

        // Update is called once per frame
        private void Update()
        {
            float delta = Time.deltaTime;

            // set bools from animation state
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isBlocking", isBlocking);
            animator.GetBool("isInAir");
            animator.SetBool("isDead", playerStatsManager.isDead);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            
            // reset flags
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            //player locomotion scripts
            // when the button is held, sprint
            // tick input first to read input
            inputHandler.TickInput(delta);
            playerLocomotionManager.HandleRollingAndSprinting(delta);
            playerLocomotionManager.HandleJumping();
            playerLocomotionManager.HandleRotation(delta);
            playerLocomotionManager.HandleFalling(delta, playerLocomotionManager.moveDirection);
            playerStatsManager.RegenerateStamina();


            // check for interactable objects
            CheckForInteractableObject();
            
            
        }

        // should handle rigidbody movement
        protected override void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;
           
            playerLocomotionManager.HandleMovement(delta);
            playerLocomotionManager.HandleFalling(delta, playerLocomotionManager.moveDirection);
            playerLocomotionManager.HandleRotation(delta);
            playerFXManager.HandleAllBuildUpEffects();
        }

        // use to reset flags
        // should be used to update camera
        private void LateUpdate()
        {
            base.FixedUpdate();
            // can olnly  be called once per frame
            inputHandler.rollFlag = false;
            inputHandler.rightbumper_Input = false;
            inputHandler.righttrigger_Input = false;
            inputHandler.lefttrigger_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.aInput = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;
            inputHandler.rightStick_Left_Input = false;
            inputHandler.rightStick_Right_Input = false;

            var delta = Time.fixedDeltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
            
            if (isInAir) playerLocomotionManager.inAirTimer = playerLocomotionManager.inAirTimer + Time.deltaTime;
        }

        #region Player Interactions
        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            // look for any object that is interactable
            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f,
                    cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        // set the UI text to interactables text
                        interactableUI.interactableText.text = interactableText;
                        // set the pop up to true
                        interactableUIGameObject.SetActive(true);
                        if (inputHandler.aInput)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.aInput)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            // freeze character first to prevent "ice skating effect"
            playerLocomotionManager.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Opening A Lid", true);

        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            // need to make sure player is facing the wall irection
            // freeze player
            playerLocomotionManager.rigidbody.velocity = Vector3.zero; // stops player from sliding
            Vector3 rotationDirection = fogWallEntrance.transform.forward; // face toward wall
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            // rotate over time
            
            playerAnimatorManager.PlayTargetAnimation("Big Jump", true);
        }
        
        #endregion
    }
}