using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isUsingRightHand = "isUsingRightHand";
    public bool isUsingRightHandStatus = false;
    
    public string isUsingLeftHand = "isUsingLeftHand";
    public bool isUsingLeftHandStatus = false;

    public string isInvulnerable = "isInvulnerable";
    public bool isInvulnerableStatus = false;
    
    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;

    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringSpellStatus = false;

    public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus;

    public string canRotateBool = "canRotate";
    // when entering empty status, activate rotating
    public bool canRotateStatus = true;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInvulnerable, isInvulnerableStatus);
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isUsingRightHand, isUsingRightHandStatus);
        animator.SetBool(isUsingLeftHand,isUsingLeftHandStatus);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //  animator.SetBool("isInteracting", false);
    // }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}