using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{

    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        // only have animation applied if is interacting is true
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
}
}
