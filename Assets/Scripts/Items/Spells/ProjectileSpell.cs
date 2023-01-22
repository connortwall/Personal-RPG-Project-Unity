using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    [Header("Projectile Damage")]
    public float baseDamage;

    [Header("Projectile Physics")]
    public float projectileForwardVelocity;
    public float projectileUpwardVelocity;
    public float projectileMass;
    public bool isAffectedByGravity;
    private Rigidbody rigidbody;


    public override void AttemptToCastSpell(
        PlayerAnimatorManager playerAnimatorManager,
        PlayerStats playerStats,
        WeaponSlotManager weaponSlotManager
    )
    {
        base.AttemptToCastSpell(playerAnimatorManager,playerStats,weaponSlotManager);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
        instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        // play animation to cast spell
        playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(
        PlayerAnimatorManager playerAnimatorManager,
        PlayerStats playerStats,
        CameraHandler cameraHandler,
        WeaponSlotManager weaponSlotManager)
    {
        base.SuccessfullyCastSpell(playerAnimatorManager, playerStats, cameraHandler, weaponSlotManager);
        GameObject instatiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
        rigidbody = instatiatedSpellFX.GetComponent<Rigidbody>();
        // spell daamage collider, damage calculations
        // spellDamagecollider = instatiatedSpellFX.GetComponent<SpellDamageCollider>();

        // if targeting an enemy
        if (cameraHandler.currentLockOnTarget != null)
        {
            instatiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
        }
        else
        {
            // judge height of projectile by camera, and direction by characters facing direction
            instatiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
        }
        
        // add velocity going forward
        rigidbody.AddForce(instatiatedSpellFX.transform.forward * projectileForwardVelocity);
        rigidbody.AddForce(instatiatedSpellFX.transform.up * projectileUpwardVelocity);
        rigidbody.useGravity = isAffectedByGravity;
        rigidbody.mass = projectileMass;
        // unparent he game object
        instatiatedSpellFX.transform.parent = null;
    }
}
}
