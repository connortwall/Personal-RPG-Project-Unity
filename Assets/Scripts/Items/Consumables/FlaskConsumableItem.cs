using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
public class FlaskConsumableItem : ConsumableItem
{
    [Header("Flask Type")] 
    public bool estusFlask;
    public bool ashenFlask;

    [Header(("Recovery Amount"))] 
    public int healthRecoveryAmount;

    public int focusPointAmount;
    
    [Header(("Recovery FX"))] 
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerFXManager playerFXManager)
    {
        // play both logic from deriving and specific to this
        base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerFXManager);
        GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        // add health ot fp
        playerFXManager.currentParticleFX = recoveryFX;
        playerFXManager.amountToBeHealed = healthRecoveryAmount;
        playerFXManager.instantiatedFXModel = flask;
        // instantion flask in hand and play animation
        // hide weapon when playing animation
        weaponSlotManager.rightHandSlot.UnloadWeapon();
        // play recovery fx
    }
}
}
