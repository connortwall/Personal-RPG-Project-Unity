using UnityEngine;

namespace CW
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private int vertical;
        private int horizontal;
        public bool canRotate;

        // TODO: understand difference between initialize and awake
        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
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
            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }
        
        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        // enable invulnerability for the character
        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }
        
        // disable invulnerability for the character
        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);

        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            // using no animation bc the critical attack resulting in death has special sequence of instant death
            // (rather than taking damage then dying)
            // alternatively check if isInteracting, don't play falling and death animationn
            playerStats.TakeDamage(playerManager.pendingCriticalDamage, false);
            // reset pending damage
            playerManager.pendingCriticalDamage = 0;
        }
        
        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            // keep drag 0
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            // keep y on zero in case animation is funky
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}