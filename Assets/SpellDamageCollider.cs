using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace CW
{
   

public class SpellDamageCollider : DamageCollider
{
   // when projectile hits something
   public GameObject impactParticles;
   // projectile itself
   public GameObject projectileParticles;
   // particles left behind in hand
   public GameObject muzzleParticles;

   private bool hasCollided = false;
   private Rigidbody rigidbody;
   private CharacterStats spellTarget;
   
   // used to rotate impact particles
   private Vector3 impactNormal;
   
   // if rigidbody not triggering, sometimes must call on awake
   private void Awake()
   {
      rigidbody = GetComponent<Rigidbody>();
   }

   private void Start()
   {
      projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
      projectileParticles.transform.parent = transform;
      if (muzzleParticles)
      {
         muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
         Destroy(muzzleParticles, 2f); // how long muzzle particles last
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (!hasCollided)
      {
         spellTarget = other.transform.GetComponent<CharacterStats>();

         if (spellTarget != null)
         {
            spellTarget.TakeDamage(currentWeaponDamage, true);
         }
         hasCollided = true;
         // instantiate impact of explosion impact
         impactParticles = Instantiate(impactParticles, transform.position,
            Quaternion.FromToRotation(Vector3.up, impactNormal));
         
         Destroy(projectileParticles);
         Destroy(impactParticles, 4f);
         Destroy(gameObject, 4f);
      }
   }
}
}
