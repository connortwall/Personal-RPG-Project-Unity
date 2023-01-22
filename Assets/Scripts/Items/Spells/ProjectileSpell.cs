using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
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
        PlayerStats playerStats)
    {
        base.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
    }
}
}
