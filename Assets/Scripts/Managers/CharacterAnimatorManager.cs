using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CW
{

    public class CharacterAnimatorManager : MonoBehaviour
    {
        [HideInInspector]
        public Animator animator;

        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        public bool canRotate;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        private bool handIKWeightsReset = false;
        
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        // only have animation applied if is interacting is true
        animator.applyRootMotion = isInteracting;
        animator.SetBool("canRotate", canRotate);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isRotatingWithRootMotion", true);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        // using no animation bc the critical attack resulting in death has special sequence of instant death
        // alternatively check if isInteracting, don't play falling and death animationn
        characterStatsManager.TakeDamage(characterManager.pendingCriticalDamage, 0,false);
        // reset pending damage
        characterManager.pendingCriticalDamage = 0;
    }
    
    public virtual void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public virtual void StopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }
        
    public virtual void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    // enable invulnerability for the character
    public virtual void EnableIsInvulnerable()
    {
        animator.SetBool("isInvulnerable", true);
    }
        
    // disable invulnerability for the character
    public virtual void DisableIsInvulnerable()
    {
        animator.SetBool("isInvulnerable", false);

    }
    
    // can use these as animation events bc they exist on thw same level as animation events
    public virtual void EnableIsParrying()
    {
        characterManager.isParrying = true;
    }
           
    public virtual void DisableIsParrying()
    {
        characterManager.isParrying = false;
    }
    
    public virtual void EnableCanBeRiposted()
    {
        characterManager.canBeRiposted = true;
    }
        
    public virtual void DisableCanBeRiposted()
    {
        characterManager.canBeRiposted = false;
    }

    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHandingWeapon)
    { 
        // apply hand IK, assign hand IK to targets (weapons)
        if (isTwoHandingWeapon)
        {
            rightHandConstraint.data.target = rightHandIKTarget.transform;
            rightHandConstraint.data.targetPositionWeight = 1; // assign this from a weapon variable if desired
            rightHandConstraint.data.targetRotationWeight = 1;

            leftHandConstraint.data.target = leftHandIKTarget.transform;
            leftHandConstraint.data.targetPositionWeight = 1;
            leftHandConstraint.data.targetRotationWeight = 1;
        }
        // if not, disable hand IK
        else
        {
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }

        rigBuilder.Build();
    }

    public void CheckHandIKWeight(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget,
        bool isTwoHandingWeapon)
    {
        if (characterManager.isInteracting)
        {
            return;
        }

        // if need to reset IK
        if (handIKWeightsReset)
        {
            handIKWeightsReset = false;
            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.target = rightHandIKTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }
            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.target = leftHandIKTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
    }

    public virtual void EraseHandIKForWeapon()
    {
        // reset hand IK to 0
        if (rightHandConstraint.data.target != null)
        {
            handIKWeightsReset = true;
            rightHandConstraint.data.targetPositionWeight = 0;
            rightHandConstraint.data.targetRotationWeight = 0;
        }
        if (leftHandConstraint.data.target != null)
        {
            handIKWeightsReset = true;
            leftHandConstraint.data.targetPositionWeight = 0;
            leftHandConstraint.data.targetRotationWeight = 0;
        }
     }

}
}
