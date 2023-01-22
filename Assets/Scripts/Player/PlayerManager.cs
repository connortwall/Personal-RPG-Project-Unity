using UnityEngine;

namespace CW
{
    public class PlayerManager : CharacterManager
    {
        private Animator anim;
        private CameraHandler cameraHandler; 
        private InputHandler inputHandler;
        private PlayerStats playerStats;
        private PlayerAnimatorManager playerAnimatorManager;
        private PlayerLocomotion playerLocomotion;
        
        private InteractableUI interactableUI; 
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
                
        public bool isInteracting;
        
        [Header("Player Flags")]
        public bool isSprinting;
        public bool isGrounded;
        public bool isInAir;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;
        

        

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backstabCollider = GetComponentInChildren<CriticalDamageCollider>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerStats = GetComponent<PlayerStats>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();

        }
        

        // Update is called once per frame
        private void Update()
        {
            float delta = Time.deltaTime;

            // set bools from animation state
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            isFiringSpell = anim.GetBool("isFiringSpell");
            
            // manager = animator
            anim.SetBool("isBlocking", isBlocking);
            isInAir = anim.GetBool("isInAir");
            // update animator is dead bool (in associated character managers)
            anim.SetBool("isDead", playerStats.isDead);
            playerAnimatorManager.canRotate = anim.GetBool("canRotate");
            
            // reset flags
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            //player locomotion scripts
            // when the button is held, sprint
            // tick input first to read input
            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerLocomotion.HandleRotation(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerStats.RegenerateStamina();


            // check for interactable objects
            CheckForInteractableObject();
            
            
        }

        // should handle rigidbody movement
        private void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;
           
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

        }

        // use to reset flags
        // should be used to update camera
        private void LateUpdate()
        {
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
            
            if (isInAir) playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
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
            playerLocomotion.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Opening A Lid", true);

        }
        
        #endregion
    }
}