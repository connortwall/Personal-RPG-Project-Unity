
using UnityEngine;

namespace CW
{
   
public class EnemyAnimationManager : AnimatorManager
{
   private EnemyManager enemyManager;
      
   private void Awake()
   {
      anim = GetComponent<Animator>();
      enemyManager = GetComponentInParent<EnemyManager>();
   }

   private void OnAnimatorMove()
   {

      // every time animator plays animatios with motion, ecenters model on game object
   float delta = Time.deltaTime;
   enemyManager.enemyRigidbody.drag = 0;
   Vector3 deltaPosition = anim.deltaPosition;
   deltaPosition.y = 0;
   Vector3 velocity = deltaPosition / delta;
   enemyManager.enemyRigidbody.velocity = velocity;
   }
}
}
