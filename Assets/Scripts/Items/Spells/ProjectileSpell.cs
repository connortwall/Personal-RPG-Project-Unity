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
        PlayerStatsManager playerStatsManager,
        PlayerWeaponSlotManager playerWeaponSlotManager
    )
    {
        base.AttemptToCastSpell(playerAnimatorManager,playerStatsManager,playerWeaponSlotManager);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerWeaponSlotManager.rightHandSlot.transform);
        instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        // play animation to cast spell
        playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(
        PlayerAnimatorManager playerAnimatorManager,
        PlayerStatsManager playerStatsManager,
        CameraHandler cameraHandler,
        PlayerWeaponSlotManager playerWeaponSlotManager)
    {
        base.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
        GameObject instatiatedSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
        SpellDamageCollider spellDamageCollider = instatiatedSpellFX.GetComponent<SpellDamageCollider>();
        spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
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
            instatiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStatsManager.transform.eulerAngles.y, 0);
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
