using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
[CreateAssetMenu(menuName = "Items/Consumables/Bomb Item")]
public class BombConsumableItem : ConsumableItem
{
    [Header("Velocity")] 
    public int upwardVelocity = 50;
    public int forwardVelocity = 50;
    public int bombMass = 1;

    [Header("Live Bomb Model")] 
    public GameObject liveBombModel;

    [Header("Base Damage")] 
    public int baseDamage = 200;
    public int explosiveDamage = 75;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager playerWeaponSlotManager,
        PlayerFXManager playerFXManager)
    {
        if (currentItemAmount > 0)
        {
            playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
            playerAnimatorManager.PlayTargetAnimation(consumeAnimation,true);
            GameObject bombModel = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform.position,
                Quaternion.identity, playerWeaponSlotManager.rightHandSlot.transform);
            playerFXManager.instantiatedFXModel = bombModel;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Shrugging", true);
        }
    }
}

}