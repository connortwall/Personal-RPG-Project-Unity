
using UnityEngine;

namespace CW
{
   
public class EnemyAnimationManager : AnimatorManager
{
   private EnemyManager enemyManager;
   private EnemyStats enemyStats;
      
   private void Awake()
   {
      anim = GetComponent<Animator>();
      enemyManager = GetComponentInParent<EnemyManager>();
      enemyStats = GetComponentInParent<EnemyStats>();
   }
   
   public override void TakeCriticalDamageAnimationEvent()
   {
      // using no animation bc the critical attack resulting in death has special sequence of instant death
      // (rather than taking damage then dying)
      // alternatively check if isInteracting, don't play falling and death animationn
      enemyStats.TakeDamage(enemyManager.pendingCriticalDamage, false);
      // reset pending damage
      enemyManager.pendingCriticalDamage = 0;
   }

   // want a delay on receiving exp
   public void AwardExpOnDeath()
   {
      PlayerStats playerStats = FindObjectOfType<PlayerStats>();
      // use for loop if multiple players
      ExpCountBar expCountBar = FindObjectOfType<ExpCountBar>();
      
      if (playerStats != null)
      {
         playerStats.AddExp(enemyStats.expAwardedOnDeath);
         
         if (expCountBar != null)
         {
            expCountBar.SetExpCountText(playerStats.expCount);
         }
      }
      
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
