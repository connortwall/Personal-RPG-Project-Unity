using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    

    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect")]
    public class CureConsumableItem : ConsumableItem
{
    [Header(("Recovery FX"))] 
    public GameObject CureFX;

    [Header("Cure FX")] 
    public bool curePoison;
    // other cure effects optional
    
    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager playerWeaponSlotManager, PlayerFXManager playerFXManager)
    {
        // play both logic from deriving consumable and specific to this, (starting with base)
        base.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerFXManager);
        GameObject cure = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform);
        // add health ot fp
        playerFXManager.currentParticleFX = CureFX;
        playerFXManager.instantiatedFXModel = cure;
        // instantion flask in hand and play animation
        if (curePoison)
        {
            playerFXManager.poisonBuildUpStatus = 0;
            playerFXManager.poisonBuildUpLimit = playerFXManager.defaultPoisonBuildUpLimit;
            playerFXManager.isPoisoned = false;

            if (playerFXManager.currentPoisonParticleFX != null)
            {
                Destroy(playerFXManager.currentPoisonParticleFX);
            }
        }
        // hide weapon when playing animation
        playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
        // play recovery fx
    }
}
}
