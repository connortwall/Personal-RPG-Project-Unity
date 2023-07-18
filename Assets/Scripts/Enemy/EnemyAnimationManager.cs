
using UnityEngine;

namespace CW
{
   
public class EnemyAnimationManager : CharacterAnimatorManager
{
   private EnemyManager enemyManager;
   private EnemyFXManager enemyFXManager;
   private EnemyBossManager enemyBossManager;
   
   
      
   protected override void Awake()
   {
      base.Awake();
      animator = GetComponent<Animator>();
      enemyManager = GetComponent<EnemyManager>();
      enemyFXManager = GetComponent<EnemyFXManager>();
      enemyBossManager = GetComponent<EnemyBossManager>();
   }

   // want a delay on receiving exp
   public void AwardExpOnDeath()
   {
      PlayerStatsManager playerStatsManager = FindObjectOfType<PlayerStatsManager>();
      // use for loop if multiple players
      ExpCountBar expCountBar = FindObjectOfType<ExpCountBar>();
      
      if (playerStatsManager != null)
      {
         playerStatsManager.AddExp(characterStatsManager.expAwardedOnDeath);
         
         if (expCountBar != null)
         {
            expCountBar.SetExpCountText(playerStatsManager.expCount);
         }
      }
      
   }

   public void InstantiateBossParticleFX()
   {
      BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
      GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
   }

   public void PlayWeaponTrailFX()
   {
      //enemyFXManager.PlayWeaponFX(false);
   }
   private void OnAnimatorMove()
   {

      // every time animator plays animatios with motion, ecenters model on game object
   float delta = Time.deltaTime;
   enemyManager.enemyRigidbody.drag = 0;
   Vector3 deltaPosition = animator.deltaPosition;
   deltaPosition.y = 0;
   Vector3 velocity = deltaPosition / delta;
   enemyManager.enemyRigidbody.velocity = velocity;

   if (enemyManager.isRotatingWithRootMotion)
   {
      // use *= to rotate over time
      enemyManager.transform.rotation *= animator.deltaRotation;
   }
   }
   
}
}
