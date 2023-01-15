using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    

public class PlayerEquipmentManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerInventory playerInventory;
    private BlockingCollider blockingCollider;

    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
    }

    public void EnableBlockingCollider()
    {
        // if two hganding
        if (inputHandler.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);

        }
        else
        {
            // normally block with left weapon
            blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);

        }
        
      blockingCollider.EnableBlockingCollider();
    }
    
    public void DisableBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
}
