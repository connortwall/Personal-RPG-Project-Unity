using UnityEditor.Rendering;
using UnityEngine;

namespace CW
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        private InputHandler inputHandler;
        private PlayerManager playerManager;
        private PlayerStatsManager playerStatsManager;
        private PlayerLocomotionManager playerLocomotionManager;
        private int vertical;
        private int horizontal;

        protected override void Awake()
        {
            base.Awake();
            inputHandler = GetComponentInParent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerLocomotionManager = GetComponentInParent<PlayerLocomotionManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        }
        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical

            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }

            #endregion

            #region Horizontal

            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }

            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            // Send float values to the Animator to affect transitions.
            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void DisableCollision()
        {
            playerLocomotionManager.characterCollider.enabled = false;
            playerLocomotionManager.characterCollisionBlockerCollider.enabled = false;
        }
        
        public void EnableCollision()
        {
            playerLocomotionManager.characterCollider.enabled = true;
            playerLocomotionManager.characterCollisionBlockerCollider.enabled = true;
        }
        
        private void OnAnimatorMove()
        {
            if (characterManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            // keep drag 0
            playerLocomotionManager.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            // keep y on zero in case animation is funky
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotionManager.rigidbody.velocity = velocity;
        }
    }
}