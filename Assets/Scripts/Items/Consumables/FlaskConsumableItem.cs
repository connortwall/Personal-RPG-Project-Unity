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
    public int healthRecoverAmount;

    public int focusPointRecoverAmount;
    
    [Header(("Recovery FX"))] 
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager playerWeaponSlotManager, PlayerFXManager playerFXManager)
    {
        // play both logic from deriving consumable and specific to this, (starting with base)
        base.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerFXManager);
        GameObject flask = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform);
        // add health ot fp
        playerFXManager.currentParticleFX = recoveryFX;
        playerFXManager.amountToBeHealed = healthRecoverAmount;
        playerFXManager.instantiatedFXModel = flask;
        // instantion flask in hand and play animation
        // hide weapon when playing animation
        playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
        // play recovery fx
    }
}
}
